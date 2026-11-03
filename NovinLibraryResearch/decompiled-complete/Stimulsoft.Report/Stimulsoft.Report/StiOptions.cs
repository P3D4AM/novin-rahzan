using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.App;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Components.ShapeTypes;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Dashboard.Styles.Cards;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Adapters.Azure;
using Stimulsoft.Report.Dictionary.Adapters.Google;
using Stimulsoft.Report.Dictionary.Adapters.OnlineService;
using Stimulsoft.Report.Dictionary.DataSources.Google;
using Stimulsoft.Report.Dictionary.Databases.Azure;
using Stimulsoft.Report.Dictionary.Databases.Google;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.QuickButtons;
using Stimulsoft.Report.SaveLoad;
using Stimulsoft.Report.Viewer;

namespace Stimulsoft.Report;

public sealed class StiOptions
{
	public static class Appearance
	{
		[DefaultValue(true)]
		[StiSerializable]
		public static bool ShowTooltipsHelp { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool ShowFormsHelp { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool ShowFormsHints { get; set; } = true;

	}

	public sealed class Configuration
	{
		private static bool isWeb;

		[DefaultValue("")]
		[Description("Gets or sets a name of the file which is used for settings of a report.")]
		[StiSerializable]
		public static string DefaultReportSettingsPath { get; set; }

		[DefaultValue("")]
		[Description("Gets or sets a name of the file which is used for configuration of a report.")]
		[StiSerializable]
		public static string DefaultReportConfigPath { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value which indicates whether it is necessary to use the path from the register for localization files search.")]
		[StiSerializable]
		public static bool SearchLocalizationFromRegistry
		{
			get
			{
				return StiLocalization.SearchLocalizationFromRegistry;
			}
			set
			{
				StiLocalization.SearchLocalizationFromRegistry = value;
			}
		}

		[DefaultValue("Localization")]
		[Description("Gets or sets string containing path to directory in which files with localized resource are located.")]
		[StiSerializable]
		public static string DirectoryLocalization
		{
			get
			{
				return StiLocalization.DirectoryLocalization;
			}
			set
			{
				StiLocalization.DirectoryLocalization = value;
			}
		}

		[DefaultValue("")]
		[Description("Gets or sets name of file with localized resource.")]
		[StiSerializable]
		public static string Localization
		{
			get
			{
				return StiLocalization.Localization;
			}
			set
			{
				StiLocalization.Localization = value;
			}
		}

		public static string ApplicationDirectory { get; set; }

		public static bool IsWeb
		{
			get
			{
				return isWeb;
			}
			set
			{
				if (isWeb != value)
				{
					isWeb = value;
					if (value)
					{
						InitWeb();
					}
				}
			}
		}

		public static bool IsWinForms => ((ReadOnlyCollectionBase)(object)Application.OpenForms).Count > 0;

		public static bool IsWPF { get; set; }

		[Obsolete("The 'IsXbap' property is deprecated!")]
		public static bool IsXbap { get; set; }

		public static bool IsServer { get; set; }

		public static void InitWeb()
		{
			Engine.LogEnabled = false;
			Engine.HideMessages = true;
		}

		static Configuration()
		{
			DefaultReportSettingsPath = string.Empty;
			DefaultReportConfigPath = string.Empty;
			try
			{
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				ApplicationDirectory = ((entryAssembly != null) ? Path.GetDirectoryName(entryAssembly.Location) : AppDomain.CurrentDomain.BaseDirectory);
			}
			catch
			{
			}
		}
	}

	public sealed class Designer
	{
		public static class Blockly
		{
			[DefaultValue("")]
			[StiSerializable]
			public static string WebView2UserDataFolder { get; set; } = string.Empty;

		}

		public static class BusinessObjectEditor
		{
			[DefaultValue(true)]
			[StiSerializable]
			public static bool BusinessObjectVisible { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool MasterComponentVisible { get; set; } = true;

		}

		public sealed class ComponentsTypes
		{
			public sealed class SimpleComponents
			{
				public static Type Text { get; set; } = typeof(StiText);


				public static Type RickText { get; set; } = typeof(StiRichText);


				public static Type Image { get; set; } = typeof(StiImage);


				public static Type Map { get; set; } = typeof(StiMap);


				public static Type CheckBox { get; set; } = typeof(StiCheckBox);

			}

			public sealed class ComplexComponents
			{
				public static Type Container { get; set; } = typeof(StiContainer);


				public static Type Panel { get; set; } = typeof(Stimulsoft.Report.Components.StiPanel);


				public static Type SubReport { get; set; } = typeof(StiSubReport);

			}

			public sealed class Bands
			{
				public static Type ReportTitleBand { get; set; } = typeof(StiReportTitleBand);


				public static Type HeaderBand { get; set; } = typeof(StiHeaderBand);


				public static Type GroupHeaderBand { get; set; } = typeof(StiGroupHeaderBand);


				public static Type DataBand { get; set; } = typeof(StiDataBand);


				public static Type GroupFooterBand { get; set; } = typeof(StiGroupFooterBand);


				public static Type FooterBand { get; set; } = typeof(StiFooterBand);


				public static Type ChildBand { get; set; } = typeof(StiChildBand);

			}
		}

		public sealed class CrossTab
		{
			public static Color[] StyleColors { get; set; } = new Color[16]
			{
				Color.White,
				Color.DarkGray,
				Color.PeachPuff,
				Color.Plum,
				Color.LightCoral,
				Color.SkyBlue,
				Color.LightSeaGreen,
				Color.LightGreen,
				Color.YellowGreen,
				Color.Wheat,
				Color.Khaki,
				ColorTranslator.FromHtml("#0bac45"),
				ColorTranslator.FromHtml("#b5a1dd"),
				ColorTranslator.FromHtml("#ffc000"),
				ColorTranslator.FromHtml("#ed7d31"),
				ColorTranslator.FromHtml("#239fd9")
			};

		}

		public class CustomHelp
		{
			[DefaultValue(false)]
			[StiSerializable]
			public static bool ShowCustomHelp { get; set; }

			public static event EventHandler RunCustomHelp;

			public static void InvokeRunCustomHelp(object sender)
			{
				CustomHelp.RunCustomHelp?.Invoke(sender, EventArgs.Empty);
			}
		}

		public static class DataBandEditor
		{
			[DefaultValue(true)]
			[StiSerializable]
			public static bool DataSourceVisible { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool DataRelationVisible { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool MasterComponentVisible { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool SortVisible { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool FiltersVisible { get; set; } = true;

		}

		public static class Editors
		{
			public static class ScriptEditor
			{
				[DefaultValue(true)]
				[StiSerializable]
				public static bool BlocksScriptEditorByDefault
				{
					get
					{
						return StiSettings.GetBool("Designer", "BlocksScriptEditorByDefault", nullValue: true);
					}
					set
					{
						StiSettings.Set("Designer", "BlocksScriptEditorByDefault", value);
					}
				}
			}

			public static class ChartEditor
			{
				[DefaultValue(true)]
				[StiSerializable]
				public static bool SimpleEditorByDefault
				{
					get
					{
						return StiSettings.GetBool("Designer", "SimpleChartEditorByDefault", nullValue: true);
					}
					set
					{
						StiSettings.Set("Designer", "SimpleChartEditorByDefault", value);
					}
				}

				[DefaultValue(false)]
				[StiSerializable]
				public static bool ShowRunChartWizard
				{
					get
					{
						return StiSettings.GetBool("Designer", "ShowRunChartWizard", nullValue: false);
					}
					set
					{
						StiSettings.Set("Designer", "ShowRunChartWizard", value);
					}
				}
			}

			public static class GaugeEditor
			{
				public static bool AllowOldEditor
				{
					get
					{
						return StiGaugeV2InitHelper.AllowOldEditor;
					}
					set
					{
						StiGaugeV2InitHelper.AllowOldEditor = value;
					}
				}
			}

			public static class ImageEditor
			{
				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowImageTabPage { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDataColumnTabPage { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowImageDataTabPage { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowImageURLTabPage { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowFileTabPage { get; set; } = true;

			}

			public static class SystemRichTextEditor
			{
				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDataColumnTabPage { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDataTabPage { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowURLTabPage { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowFileTabPage { get; set; } = true;

			}

			public static class SystemTextEditor
			{
				[DefaultValue(false)]
				[StiSerializable]
				public static bool HighlightTextInSystemTextEditor { get; set; }
			}

			[DefaultValue(false)]
			[StiSerializable]
			public static bool AllowConnectToDataInGallery { get; set; }

			public static event StiEditCustomConnectionEventHandler EditCustomConnection;

			public static event StiEditDataStoreAdapterEventHandler EditDataStoreAdapter;

			public static void InvokeEditCustomConnection(StiEditCustomConnectionEventArgs e)
			{
				Editors.EditCustomConnection?.Invoke(null, e);
			}

			public static void InvokeEditDataStoreAdapter(StiEditDataStoreAdapterEventArgs e)
			{
				Editors.EditDataStoreAdapter?.Invoke(null, e);
			}
		}

		public sealed class MainMenu
		{
			public delegate StiImportResult ImportActiveReportsDelegate(byte[] bytes);

			public delegate StiImportResult ImportComponentOneReportsDelegate(byte[] bytes);

			public delegate StiImportResult ImportCrystalReportDelegate(byte[] bytes);

			public delegate StiImportResult ImportFastReportsDelegate(byte[] bytes);

			public delegate StiImportResult ImportReportSharpShooterDelegate(byte[] bytes);

			public delegate StiImportResult ImportRtfDelegate(byte[] bytes);

			public delegate StiImportResult ImportTelerikReportsDelegate(byte[] bytes);

			public delegate StiImportResult ImportVisualFoxProDelegate(byte[] dataBytes, byte[] memoBytes);

			public delegate StiImportResult ImportMicrosoftReportingServicesDelegate(byte[] bytes);

			public static ImportActiveReportsDelegate ImportActiveReports;

			public static ImportComponentOneReportsDelegate ImportComponentOneReports;

			public static ImportCrystalReportDelegate ImportCrystalReport;

			public static ImportFastReportsDelegate ImportFastReports;

			public static ImportReportSharpShooterDelegate ImportReportSharpShooter;

			public static ImportRtfDelegate ImportRtf;

			public static ImportTelerikReportsDelegate ImportTelerikReports;

			public static ImportVisualFoxProDelegate ImportVisualFoxPro;

			public static ImportMicrosoftReportingServicesDelegate ImportMicrosoftReportingServices;

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowHelpAboutProgramm = true;

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFile { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFileReportNew { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFilePageNew { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFileFormNew { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFileDashboardNew { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFileScreenNew { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFileReportOpen { get; set; } = true;


			[Obsolete("Field 'ShowFileReportOpenFromGoogleDocs' is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowFileReportOpenFromGoogleDocs { get; set; }

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFilePageOpen { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFileReportSave { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFileReportSaveAs { get; set; } = true;


			[Obsolete("Field 'ShowFileReportSaveAsToGoogleDocs' is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowFileReportSaveAsToGoogleDocs { get; set; }

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFilePageSaveAs { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFilePageDelete { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFileReportPreview { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFilePageSetup { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFileReportSetup { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowSelectLanguage { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFileRecentFiles { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowOnlyExistingRecentFiles { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFileExit { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowEdit { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowEditUndo { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowEditRedo { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowEditCut { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowEditCopy { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowEditPaste { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowEditDelete { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowEditSelectAll { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowEditClearContents { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowView { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowViewNormal { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowViewPageBreakPreview { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowViewShowGrid { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowViewAlignToGrid { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowViewShowHeaders { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowViewShowRulers { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowViewShowOrder { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowViewToolbars { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowViewPanels { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowTools { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowToolsDataStore { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowToolsPagesManager { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowToolsServicesConfigurator { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowToolsOptions { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowHelp { get; set; } = true;


			[DefaultValue("https://stimulsoft.com/")]
			[StiSerializable]
			public static string ProductHomePage { get; set; } = "https://stimulsoft.com/";


			[DefaultValue("https://www.stimulsoft.com/")]
			[StiSerializable]
			public static string ProductHomePageWPF { get; set; } = "https://www.stimulsoft.com/";


			[DefaultValue("https://www.stimulsoft.com/en/faq/reports-net")]
			[StiSerializable]
			public static string FaqPage { get; set; } = "https://www.stimulsoft.com/en/faq/reports-net";


			[DefaultValue("https://www.stimulsoft.com/en/faq/reports-net")]
			public static string FaqPageWPF { get; set; } = "https://www.stimulsoft.com/en/faq/reports-net";


			[DefaultValue("mailto:support@stimulsoft.com")]
			[StiSerializable]
			public static string Support { get; set; } = "mailto:support@stimulsoft.com";


			[DefaultValue("https://www.stimulsoft.com/en/support")]
			[StiSerializable]
			public static string SupportPage { get; set; } = "https://www.stimulsoft.com/en/support";


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowHelpProductHomePage { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowHelpFaqPage { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowHelpSupport { get; set; } = true;

		}

		public class NewReport
		{
			[DefaultValue(StiNewReportDictionaryBehavior.NewDictionary)]
			[StiSerializable]
			public static StiNewReportDictionaryBehavior DictionaryBehavior
			{
				get
				{
					StiSettings.Load();
					return (StiNewReportDictionaryBehavior)StiSettings.Get("StiDesigner", "NewReportDictionaryBehavior", StiNewReportDictionaryBehavior.NewDictionary);
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("StiDesigner", "NewReportDictionaryBehavior", value);
					StiSettings.Save();
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowRegisterDataStoreFromOldReportInNewReport { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowRegisterDatabasesFromOldReportInNewReport { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowRegisterDataSourcesFromOldReportInNewReport { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowRegisterRelationsFromOldReportInNewReport { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowRegisterVariablesFromOldReportInNewReport { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowRegisterResourcesFromOldReportInNewReport { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowRegisterRestrictionsFromOldReportInNewReport { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowRegisterBusinessObjectsFromOldReportInNewReport { get; set; } = true;

		}

		public static class Panels
		{
			public static class Dictionary
			{
				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDuplicateForVariable { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDuplicateForDataSource { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDuplicateForDataColumn { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDuplicateForDataRelation { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDuplicateForDataParameter { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDuplicateForDataConnection { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDuplicateForResource { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPropertiesForVariable { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPropertiesForResource { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPropertiesForDataSource { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPropertiesForBusinessObject { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPropertiesForDataColumn { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPropertiesForDataRelation { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPropertiesForDataParameter { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPropertiesForDataConnection { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPropertiesForDataTransformation { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowEditForVariable { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowEditForResource { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowEditForDataSource { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowEditForBusinessObject { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowEditForDataColumn { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowEditForDataRelation { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowEditForDataParameter { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowEditForDataConnection { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowEditForDataTransformation { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDeleteForVariable { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDeleteForResource { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDeleteForDataSource { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDeleteForBusinessObject { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDeleteForDataColumn { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDeleteForDataRelation { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDeleteForDataParameter { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDeleteForDataConnection { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDeleteForDataTransformation { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowUseAliases { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowCreateFieldOnDoubleClick { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowCreateLabel { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowActionsButton { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowNewButton { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDeleteButton { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowEditButton { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowSaveResource { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowUpButton { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDownButton { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowSortItemsButton { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDictXmlMergeMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDictXmlExportMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDictXmlImportMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDictSaveMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDictMergeMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDictOpenMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDictNewMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowConnectionNewMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDataTransformationNewMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowRelationNewMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDataSourceNewMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowBusinessObjectNewMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowColumnNewMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDataParameterNewMenuItem { get; set; } = true;


				[DefaultValue(false)]
				[StiSerializable]
				public static bool ShowDataSourcesNewMenuItem { get; set; }

				[DefaultValue(false)]
				[StiSerializable]
				public static bool ShowRelationsImportMenuItem { get; set; }

				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowSynchronizeMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowViewDataMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPropertiesMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDuplicateMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowCategoryNewMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowVariableNewMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowResourceNewMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowEditMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowMakeThisRelationActive { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDeleteMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowContextMenu { get; set; } = true;


				[DefaultValue(false)]
				[StiSerializable]
				public static bool ShowMarkUsedMenuItem { get; set; }

				[DefaultValue(false)]
				[StiSerializable]
				public static bool ShowRemoveUnusedMenuItem { get; set; }

				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowCalcColumnNewMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowExpandAllMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowCollapseAllMenuItem { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowEmptyDataSourcesCategory { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowEmptyBusinessObjectsCategory { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowEmptyVariablesCategory { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowEmptyResourcesCategory { get; set; } = true;

			}
		}

		public class Properties
		{
			public static void Hide(Type type, string propertyName)
			{
				StiPropertyGrid.HideProperty(type, propertyName);
			}

			public static void Hide(string propertyName)
			{
				StiPropertyGrid.HideProperty("All", propertyName);
			}

			public static void Show(Type type, string propertyName)
			{
				StiPropertyGrid.ShowProperty(type, propertyName);
			}

			public static void Show(string propertyName)
			{
				StiPropertyGrid.ShowProperty("All", propertyName);
			}

			public static bool IsAllowed(Type type, string propertyName)
			{
				return StiPropertyGrid.IsAllowedProperty(type, propertyName);
			}
		}

		public sealed class PropertyGrid
		{
			private static bool propertyLevelLoaded;

			private static StiLevel? propertyLevel;

			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowShowConfiguration { get; set; } = true;


			[DefaultValue(false)]
			[StiSerializable]
			public static bool AllowEditStyle { get; set; }

			[DefaultValue(null)]
			[StiSerializable]
			public static StiLevel? PropertyLevel
			{
				get
				{
					if (propertyLevelLoaded)
					{
						return propertyLevel;
					}
					StiSettings.Load();
					propertyLevelLoaded = true;
					propertyLevel = (StiLevel?)StiSettings.Get("StiDesigner", "PropertyLevel", null);
					StiPropertyGridOptions.PropertyLevel = propertyLevel;
					return propertyLevel;
				}
				set
				{
					//IL_0011: Unknown result type (might be due to invalid IL or missing references)
					//IL_0018: Unknown result type (might be due to invalid IL or missing references)
					if (!propertyLevelLoaded || propertyLevel != value)
					{
						StiSettings.Load();
						StiSettings.Set("StiDesigner", "PropertyLevel", value);
						propertyLevel = value;
						propertyLevelLoaded = true;
						StiPropertyGridOptions.PropertyLevel = propertyLevel;
						StiSettings.Save();
					}
				}
			}

			[DefaultValue(true)]
			[Description("Gets or sets a value, indicating whether to show the StiPropertyGrid description or not.")]
			[StiSerializable]
			public static bool ShowDescription
			{
				get
				{
					return StiPropertyGridOptions.ShowDescription;
				}
				set
				{
					StiPropertyGridOptions.ShowDescription = value;
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool Localizable
			{
				get
				{
					return StiPropertyGridOptions.Localizable;
				}
				set
				{
					StiPropertyGridOptions.Localizable = value;
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowPropertiesWhichUsedFromStyles { get; set; }
		}

		public sealed class Restrictions
		{
			[DefaultValue(false)]
			[StiSerializable]
			public static bool IgnoreAllowChange { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			public static bool IgnoreAllowMove { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			public static bool IgnoreAllowSelect { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			public static bool IgnoreAllowResize { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			public static bool IgnoreAllowDelete { get; set; }
		}

		public static class Ribbon
		{
			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowOfficeButton { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowLocalizationMenu { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowHelp { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowAbout { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowTabHome { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowTabHomeBarClipboard { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowTabHomeBarFont { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowTabHomeBarAlignment { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowTabHomeBarBorders { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowTabHomeBarTextFormat { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowTabHomeBarStyle { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowTabPage { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowTabPageBarPageSetup { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("ShowTabPageBarWatermarkText property is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowTabPageBarWatermarkText { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("ShowTabPageBarWatermarkImage property is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowTabPageBarWatermarkImage { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowTabLayout { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowTabLayoutBarArrange { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("ShowTabLayoutBarDockStyle property is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowTabLayoutBarDockStyle { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("ShowMainMenuPageNew property is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowTabView { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowTabViewBarViewOptions { get; set; } = true;


			[DefaultValue(false)]
			[StiSerializable]
			public static bool ShowTabPreview { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowQuickReportOpen { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowQuickReportSave { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowMainMenuNew { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowMainMenuReportNew { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("ShowMainMenuReportWizardNew property is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowMainMenuReportWizardNew { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("ShowMainMenuPageNew property is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowMainMenuPageNew { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("ShowMainMenuFormNew property is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowMainMenuFormNew { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowMainMenuReportOpen { get; set; } = true;


			[Obsolete("Field 'ShowMainMenuReportOpenFromGoogleDocs' is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowMainMenuReportOpenFromGoogleDocs { get; set; }

			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("ShowMainMenuPageOpen property is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowMainMenuPageOpen { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowMainMenuReportSave { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("ShowMainMenuSaveAs property is not used more! Please use ShowMainMenuReportSaveAs property instead.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowMainMenuSaveAs
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

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowMainMenuReportSaveAs { get; set; } = true;


			[Obsolete("Field 'ShowMainMenuReportSaveAsToGoogleDocs' is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowMainMenuReportSaveAsToGoogleDocs { get; set; }

			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("ShowMainMenuPageSaveAs property is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowMainMenuPageSaveAs { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("ShowMainMenuPageDelete property is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowMainMenuPageDelete { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowMainMenuReportPreview { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowMainMenuCheckForIssues { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowMainMenuReportSetup { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowMainMenuRecentFiles { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowMainMenuClose { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("ShowMainMenuDesigner property is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowMainMenuDesigner { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("ShowMainMenuReport property is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowMainMenuReport { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("ShowMainMenuPage property is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowMainMenuPage { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("ShowMainMenuReportPrint property is not used more!")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ShowMainMenuReportPrint { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowMainMenuOptions { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowMainMenuExit { get; set; } = true;


			[DefaultValue(false)]
			[StiSerializable]
			public static bool ShowMainMenuHelp { get; set; } = false;

		}

		public sealed class StyleDesigner
		{
			public static event StiStyleDesignerOpeningEventHandler Opening;

			public static event StiLoadCustomEventHandler LoadCustomStyle;

			public static void InvokeOpening(object sender, StiStyleDesignerOpeningEventArgs args)
			{
				StyleDesigner.Opening?.Invoke(sender, args);
			}

			public static StiBaseStyle InvokeLoadCustomStyle(string ident)
			{
				StiLoadCustomEventArgs stiLoadCustomEventArgs = new StiLoadCustomEventArgs(ident);
				StyleDesigner.LoadCustomStyle?.Invoke(null, stiLoadCustomEventArgs);
				return stiLoadCustomEventArgs.Style;
			}
		}

		public sealed class TabsContextMenu
		{
			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowPageNew { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFormNew { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowScreenNew { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowDashboardNew { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowPageDelete { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowPageSetup { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowPageOpen { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowPageSave { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowPageClone { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowPageRename { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowPageMoveLeft { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowPageMoveRight { get; set; } = true;

		}

		public sealed class Toolbars
		{
			public static class BordersToolbar
			{
				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowBorderAll { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowBorderNone { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowBorderTop { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowBorderLeft { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowBorderBottom { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowBorderRight { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowBorderShadow { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowFillBrush { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowBorderColor { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowBorderStyle { get; set; } = true;

			}

			public static class DockStyleToolbar
			{
				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDockStyleFill { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDockStyleNone { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDockStyleTop { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDockStyleLeft { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDockStyleBottom { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDockStyleRight { get; set; } = true;

			}

			public static class FormattingToolbar
			{
				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowFont { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowFontSize { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowTextBrush { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowBold { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowItalic { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowUnderline { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowAlignLeft { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowAlignCenter { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowAlignRight { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowAlignWidth { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowAlignTop { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowAlignMiddle { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowAlignBottom { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowRotateText { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowConditions { get; set; } = true;

			}

			public static class LayoutToolbar
			{
				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowAlignToGrid { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowAlignLeft { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowAlignCenter { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowAlignRight { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowAlignTop { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowAlignMiddle { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowAlignBottom { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowMakeSameSize { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowMakeSameWidth { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowMakeSameHeight { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowMakeVerticalSpacingEqual { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowMakeHorizontalSpacingEqual { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowCenterVertically { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowCenterHorizontally { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowBringToFront { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowSendToBack { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowMoveForward { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowMoveBackward { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowLock { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowLink { get; set; } = true;

			}

			public static class StandardToolbar
			{
				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowReportNew { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPageNew { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDashboardNew { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowFormNew { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowScreenNew { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowReportOpen { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowReportSave { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPageDelete { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowReportPreview { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPageSetup { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowUndo { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowRedo { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowCut { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowCopy { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPaste { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDelete { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowSelectAll { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowShowGrid { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowAlignToGrid { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowShowHeaders { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowShowRulers { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowShowOrder { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowQuickInfo { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowZoom { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowClose { get; set; } = true;

			}

			public static class StatusBar
			{
				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowUnits { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowComponentName { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowLocation { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowReportChecker { get; set; } = true;

			}

			public static class StyleToolbar
			{
				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowStyleDesigner { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowStyles { get; set; } = true;

			}

			public static class ToolsToolbar
			{
				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowModifiers { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPagesManager { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDataStore { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowConfigurator { get; set; } = true;

			}

			public static class ZoomToolbar
			{
				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowZoomModes { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowZoomSlider { get; set; } = true;

			}
		}

		public sealed class GlobalEvents
		{
			[Obsolete("Please dont use event StiOptions.Designer.GlobalEvents.PageSetup! You need use event StiOptions.Engine.GlobalEvents.RunPageSetupInDesigner instead it.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static event EventHandler PageSetup
			{
				add
				{
					Engine.GlobalEvents.RunPageSetupInDesigner += value;
				}
				remove
				{
					Engine.GlobalEvents.RunPageSetupInDesigner -= value;
				}
			}

			[Obsolete("Please dont use method StiOptions.Designer.GlobalEvents.InvokePageSetup! You need use method StiOptions.Engine.GlobalEvents.InvokeRunPageSetupInDesigner instead it.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool InvokePageSetup(StiPage page)
			{
				return Engine.GlobalEvents.InvokeRunPageSetupInDesigner(page);
			}
		}

		public sealed class Wizards
		{
			public static ArrayList Labels { get; set; } = new ArrayList();


			public static ArrayList LabelPageSizes { get; set; } = new ArrayList();


			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowNewDataSourceButton { get; set; } = true;

		}

		private static bool designerSpecificationLoaded = false;

		private static StiDesignerSpecification designerSpecification = (StiDesignerSpecification)0;

		private static Font defaultPropertyGridFont;

		private static bool hideConnectionString;

		private static StiStylesCollection styles;

		[DefaultValue(false)]
		[StiSerializable]
		public static bool AutoCorrectDataSourceName { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		public static bool AutoCorrectDataRelationName { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		public static bool AutoCorrectDataColumnName { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		public static bool AutoCorrectComponentName { get; set; } = true;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool AutoCorrectReportName { get; set; }

		[Browsable(false)]
		public static bool IsBeginnerDesigner
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Expected I4, but got Unknown
				StiDesignerSpecification val = DesignerSpecification;
				return (val - 1) switch
				{
					2 => true, 
					1 => false, 
					0 => false, 
					_ => false, 
				};
			}
		}

		[Browsable(false)]
		public static bool IsBICreatorDesigner
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Expected I4, but got Unknown
				StiDesignerSpecification val = DesignerSpecification;
				switch (val - 1)
				{
				case 2:
					return false;
				case 1:
					return true;
				case 0:
					return false;
				default:
					if (StiStimulsoftApps.IsDesktopApp)
					{
						return true;
					}
					if (string.IsNullOrEmpty(StiLicense.Key))
					{
						return false;
					}
					return StiLicenseKey.Get(StiLicense.Key).Products.All((StiLicenseProduct p) => (long)p.Ident == 12 || (long)p.Ident == 16 || (long)p.Ident == 17 || (long)p.Ident == 18 || (long)p.Ident == 21 || (long)p.Ident == 20);
				}
			}
		}

		[Browsable(false)]
		public static bool IsDeveloperDesigner
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Expected I4, but got Unknown
				StiDesignerSpecification val = DesignerSpecification;
				switch (val - 1)
				{
				case 2:
					return false;
				case 1:
					return false;
				case 0:
					return true;
				default:
					if (StiStimulsoftApps.IsDesktopApp)
					{
						return false;
					}
					if (string.IsNullOrEmpty(StiLicense.Key))
					{
						return true;
					}
					return StiLicenseKey.Get(StiLicense.Key).Products.Any((StiLicenseProduct p) => (long)p.Ident != 12 && (long)p.Ident != 16 && (long)p.Ident != 17 && (long)p.Ident != 18 && (long)p.Ident != 21 && (long)p.Ident != 20);
				}
			}
		}

		[Browsable(false)]
		internal static bool IsShowEventsTab
		{
			get
			{
				if (ShowEventsTab)
				{
					return IsDeveloperDesigner;
				}
				return false;
			}
		}

		[Browsable(false)]
		public static bool IsCodeTabVisible
		{
			get
			{
				if (CodeTabVisible)
				{
					return IsDeveloperDesigner;
				}
				return false;
			}
		}

		[Browsable(false)]
		public static bool IsPreviewReportVisible
		{
			get
			{
				if (PreviewReportVisible)
				{
					return IsDeveloperDesigner;
				}
				return false;
			}
		}

		[DefaultValue(/*Could not decode attribute arguments.*/)]
		[Description("Gets or sets a value which indicates a type of the designer users - developers or BI creators.")]
		[StiSerializable]
		public static StiDesignerSpecification DesignerSpecification
		{
			get
			{
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				if (designerSpecificationLoaded)
				{
					return designerSpecification;
				}
				StiSettings.Load();
				designerSpecificationLoaded = true;
				designerSpecification = (StiDesignerSpecification)StiSettings.Get("StiDesigner", "DesignerSpecification", (object)(StiDesignerSpecification)0);
				return designerSpecification;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Expected I4, but got Unknown
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				switch (value - 1)
				{
				case 2:
					PropertyGrid.PropertyLevel = (StiLevel)0;
					break;
				case 1:
					PropertyGrid.PropertyLevel = (StiLevel)1;
					break;
				case 0:
					PropertyGrid.PropertyLevel = (StiLevel)2;
					break;
				}
				if (!designerSpecificationLoaded || designerSpecification != value)
				{
					StiSettings.Load();
					StiSettings.Set("StiDesigner", "DesignerSpecification", value);
					designerSpecification = value;
					designerSpecificationLoaded = true;
					StiSettings.Save();
				}
			}
		}

		[DefaultValue(StiDesignerScaleMode.AutomaticScaling)]
		[Description("Gets or sets a value which indicates a scaling mode of the designer.")]
		[StiSerializable]
		public static StiDesignerScaleMode DesignerScale
		{
			get
			{
				StiSettings.Load();
				return (StiDesignerScaleMode)StiSettings.Get("StiDesigner", "DesignerScale", StiDesignerScaleMode.AutomaticScaling);
			}
			set
			{
				StiSettings.Load();
				StiSettings.Set("StiDesigner", "DesignerScale", value);
				StiSettings.Save();
			}
		}

		[DefaultValue(StiRightToLeftType.No)]
		[Description("Gets or sets a value which controls of output of objects in the right to left mode.")]
		[StiSerializable]
		public static StiRightToLeftType RightToLeft { get; set; } = StiRightToLeftType.No;


		public static bool IsRightToLeft => RightToLeft == StiRightToLeftType.Yes;

		[Description("Gets or sets value which indicates that font names will be drawing in simple view mode in font combobox in report designer.")]
		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowCustomFontDrawing { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowUseDragDrop { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowCopyControlOperation { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool ShowWatermarkInPageSetup { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool IsComponentEditorEnabled { get; set; } = true;


		[Browsable(false)]
		internal static bool AllowAuthorizeWithLicenseKey { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		public static bool IsHighlightComponentServiceEnabled { get; set; } = true;


		[StiOptionsFontHelper(2)]
		[StiSerializable]
		public static Font DefaultPropertyGridFont
		{
			get
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Expected O, but got Unknown
				object obj = defaultPropertyGridFont;
				if (obj == null)
				{
					Font val = new Font("Arial", 8f);
					defaultPropertyGridFont = val;
					obj = (object)val;
				}
				return (Font)obj;
			}
			set
			{
				defaultPropertyGridFont = value;
			}
		}

		[DefaultValue(true)]
		[StiSerializable]
		public static bool ShowSuperToolTip { get; set; } = true;


		[DefaultValue(StiQuickButtonVisibility.Always)]
		[StiSerializable]
		public static StiQuickButtonVisibility ShowQuickButtons { get; set; } = StiQuickButtonVisibility.Always;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool CanCustomizeRibbon { get; set; } = true;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool MarkComponentsWithErrors { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		public static bool AdvancedModeOfUndoRedoService { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool SortDictionaryByAliases { get; set; } = true;


		[DefaultValue(false)]
		[Description("If property equal true then main menu from designer control showes on StiDesigner form.")]
		[StiSerializable]
		public static bool ShowDesignerControlMainMenu { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		public static bool ShowEventsTab { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets value which indicates that designer called from VisualStudio.Net. Internal use only.")]
		[StiSerializable]
		public static bool RunInDesignMode { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		[Obsolete("Property 'ShowSelectTypeOfGuiOption' is obsoleted.")]
		public static bool ShowSelectTypeOfGuiOption { get; set; } = true;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool UseSimpleGlobalizationEditor { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		public static bool UseRightToLeftGlobalizationEditor { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		public static bool AllowUseWinControl { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		public static bool HideConnectionString
		{
			get
			{
				return hideConnectionString;
			}
			set
			{
				hideConnectionString = value;
				if (hideConnectionString)
				{
					Properties.Hide("ConnectionString");
					CodeTabVisible = false;
				}
				else
				{
					Properties.Show("ConnectionString");
				}
			}
		}

		[DefaultValue(0)]
		[Description("Gets or sets the maximal nested level of dictionary objects in report dictionary panel.")]
		[StiSerializable]
		public static int MaxLevelOfDictionaryObjects { get; set; }

		public static object BackgroundColor { get; set; }

		public static StiStylesCollection Styles
		{
			get
			{
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Expected O, but got Unknown
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Expected O, but got Unknown
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				//IL_007a: Expected O, but got Unknown
				//IL_008d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Expected O, but got Unknown
				//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d0: Expected O, but got Unknown
				//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ea: Expected O, but got Unknown
				//IL_0119: Unknown result type (might be due to invalid IL or missing references)
				//IL_0123: Expected O, but got Unknown
				//IL_0137: Unknown result type (might be due to invalid IL or missing references)
				//IL_0141: Expected O, but got Unknown
				//IL_015c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0166: Expected O, but got Unknown
				//IL_016c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0176: Expected O, but got Unknown
				//IL_0182: Unknown result type (might be due to invalid IL or missing references)
				//IL_018c: Expected O, but got Unknown
				//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c5: Expected O, but got Unknown
				//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d5: Expected O, but got Unknown
				object obj = styles;
				if (obj == null)
				{
					obj = new StiStylesCollection
					{
						new StiStyle("Normal", "Normal")
						{
							Brush = (StiBrush)new StiSolidBrush(Color.Transparent),
							TextBrush = (StiBrush)new StiSolidBrush(Color.Black)
						},
						new StiStyle("Bad", "Bad")
						{
							Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(255, 255, 199, 206)),
							TextBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(255, 208, 55, 5))
						},
						new StiStyle("Good", "Good")
						{
							Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(255, 198, 239, 206)),
							TextBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(255, 0, 97, 94))
						},
						new StiStyle("Neutral", "Neutral")
						{
							Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(255, 255, 235, 156)),
							TextBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(255, 174, 127, 43))
						},
						new StiStyle("Warning", "Warning")
						{
							Brush = (StiBrush)new StiSolidBrush(Color.Transparent),
							TextBrush = (StiBrush)new StiSolidBrush(Color.Red),
							Font = new Font("Arial", 8f, (FontStyle)1)
						},
						new StiStyle("Note", "Note")
						{
							Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(255, 255, 255, 204)),
							TextBrush = (StiBrush)new StiSolidBrush(Color.Black)
						}
					};
					styles = (StiStylesCollection)obj;
				}
				return (StiStylesCollection)obj;
			}
		}

		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowAsyncDataCommands { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool AutoLargeHeight { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool CalculateBarcodeValueInDesignMode { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool UseGlobalizationManager { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value which indicates that images will be preloaded in the designer.")]
		[StiSerializable]
		public static bool PreloadImageFromDataColumn { get; set; } = true;


		[Obsolete("Form 'Tip of the Day' is not used anymore.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool ShowTipsOnStartup { get; set; }

		[Obsolete("Please use StiOptions.Windows.GlobalGuiStyle instead StiOptions.Designer.IsRibbonGuiEnabled property!")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IsRibbonGuiEnabled
		{
			get
			{
				return Windows.IsRibbonGuiEnabled();
			}
			set
			{
				Windows.GlobalGuiStyle = (value ? StiGlobalGuiStyle.Office2007Blue : StiGlobalGuiStyle.Office2003Blue);
			}
		}

		[Obsolete("Please use StiOptions.Designer.ShowWatermarkInPageSetup instead StiOptions.Designer.DisableWatermarkInPageSetup property!")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool DisableWatermarkInPageSetup
		{
			get
			{
				return ShowWatermarkInPageSetup;
			}
			set
			{
				ShowWatermarkInPageSetup = value;
			}
		}

		[Obsolete("Sorry, you can't use more this property. Now only ReportChecker available for report messages.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IsCheckMessagesEnabled { get; set; }

		[Obsolete("Please use StiOptions.Designer.IgnoreOptionReportNeverSaved. Spelling error.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IgnoreOptionReportNewerSaved
		{
			get
			{
				return IgnoreOptionReportNeverSaved;
			}
			set
			{
				IgnoreOptionReportNeverSaved = value;
			}
		}

		[DefaultValue("")]
		[Description("Gets or sets the name of the configuration file of the Docking Panels.")]
		[StiSerializable]
		public static string DefaultDockingPanelsConfigPath { get; set; } = string.Empty;


		[DefaultValue("")]
		[Description("Gets or sets the name of the configuration file of the Designer.")]
		[StiSerializable]
		public static string DefaultDesignerConfigPath { get; set; } = string.Empty;


		[DefaultValue("")]
		[Description("Gets or sets the default path to 'Templates' folder of the Designer.")]
		[StiSerializable]
		public static string DefaultTemplatesPath { get; set; } = string.Empty;


		[DefaultValue("")]
		[Description("Gets or sets the default path to 'StylesCollections' folder of the Designer.")]
		[StiSerializable]
		public static string DefaultStylesCollectionsPath { get; set; } = string.Empty;


		[DefaultValue("")]
		[Description("Gets or sets the name of the configuration file of the Ribbon Designer.")]
		[StiSerializable]
		public static string DefaultRibbonDesignerConfigPath { get; set; } = string.Empty;


		[DefaultValue("")]
		[Description("Gets or sets the name of the configuration file of quick access toolbar of the Ribbon Designer.")]
		[StiSerializable]
		public static string DefaultRibbonDesignerQatConfigPath { get; set; } = string.Empty;


		[DefaultValue("")]
		[Description("Gets or sets a file name which was used for saving or for loading report template last time.")]
		[StiSerializable]
		public static string ReportSaveLoadPath { get; set; } = string.Empty;


		[DefaultValue("")]
		[Description("Gets or sets a file name which was used for saving or for loading one page of the report template last time.")]
		[StiSerializable]
		public static string PageSaveLoadPath { get; set; } = string.Empty;


		[DefaultValue("")]
		[Description("Gets or sets a file name which was used for saving or for loading component templates.")]
		[StiSerializable]
		public static string TemplatesSaveLoadPath { get; set; } = string.Empty;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowSaveReportTemplateWithEmbededData { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowSaveToSourceCode { get; set; } = true;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool AllowSaveToSourceCodeForSilverlight { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowSaveToAssembly { get; set; } = true;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool AllowSaveStandaloneReport { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		public static bool AllowSaveSourceCodeForInheritedReports { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		public static bool DontSaveFormsSettings { get; set; }

		public static int[] AutoSaveIntervals { get; set; } = new int[6] { 5, 10, 15, 20, 30, 60 };


		[DefaultValue(false)]
		[Description("Gets or sets a value which controls of whether to save configuration of a Docking Manager object or not.")]
		[StiSerializable]
		public static bool DontSaveDockingPanelsConfig { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets a value which controls saving configuration of the designer.")]
		[StiSerializable]
		public static bool DontSaveDesignerConfig { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets a value, which controls of saving configuration of the editor.")]
		[StiSerializable]
		public static bool DontSaveEditorConfig { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets a value, which controls of questioning about saving the report when the designer is closed.")]
		[StiSerializable]
		public static bool DontAskSaveReport { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		public static bool SaveReportWhenDontAskSaveReport { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowPageSaveAs { get; set; } = true;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool PlaceMRZBeforeMRTInSaveDialog { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		public static bool IgnoreOptionReportNeverSaved { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		public static bool CanDesignerChangeReportFileName { get; set; } = true;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool ReadOnlyAlertOnSave { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value indicating to show or to hide Code Tab of the report designer.")]
		[StiSerializable]
		public static bool CodeTabVisible { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value, which shows or hides a Preview Tab in the report designer.")]
		[StiSerializable]
		public static bool PreviewReportVisible { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value, which shows or hides a WPF Preview Tab in the report designer.")]
		[StiSerializable]
		public static bool WpfPreviewReportVisible { get; set; } = true;


		[DefaultValue(false)]
		[Obsolete("'StiOptions.Designer.WebPreviewReportVisible' doesn't used more!")]
		[Browsable(false)]
		public static bool WebPreviewReportVisible { get; set; }

		[DefaultValue(false)]
		[Obsolete("'StiOptions.Designer.SLPreviewReportVisible' doesn't used more!")]
		[Browsable(false)]
		public static bool SLPreviewReportVisible { get; set; }

		[DefaultValue(false)]
		[Obsolete("'StiOptions.Designer.WinRTPreviewReportVisible' doesn't used more!")]
		[Browsable(false)]
		public static bool WinRTPreviewReportVisible { get; set; }

		[DefaultValue(false)]
		[Obsolete("'StiOptions.Designer.HtmlPreviewReportVisible' doesn't used more!")]
		[Browsable(false)]
		public static bool HtmlPreviewReportVisible { get; set; }

		[DefaultValue(false)]
		[Obsolete("'StiOptions.Designer.JsPreviewReportVisible' doesn't used more!")]
		[Browsable(false)]
		public static bool JsPreviewReportVisible { get; set; }

		[DefaultValue(false)]
		[Obsolete("'StiOptions.Designer.WebPreviewReportEnabled' doesn't used more!")]
		[Browsable(false)]
		public static bool WebPreviewReportEnabled { get; set; }

		[DefaultValue(false)]
		[Obsolete("'StiOptions.Designer.HtmlPreviewReportEnabled' doesn't used more!")]
		[Browsable(false)]
		public static bool HtmlPreviewReportEnabled { get; set; }

		[DefaultValue(false)]
		[Obsolete("'StiOptions.Designer.SLPreviewReportEnabled' doesn't used more!")]
		[Browsable(false)]
		public static bool SLPreviewReportEnabled { get; set; }

		[DefaultValue(false)]
		[Obsolete("'StiOptions.Designer.WinRTPreviewReportEnabled' doesn't used more!")]
		[Browsable(false)]
		public static bool WinRTPreviewReportEnabled { get; set; }

		[DefaultValue(StiHtmlExportMode.Table)]
		[StiSerializable]
		public static StiHtmlExportMode PreviewHtmlExportMode { get; set; } = StiHtmlExportMode.Table;


		[DefaultValue(true)]
		[Description("Gets or sets a value indicating whether the Preview Window of the report will be showing as MDI window.")]
		[StiSerializable]
		public static bool ShowPreviewInMdi { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowMultiColorTitle { get; set; } = true;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool ShowFullFileNameInTitle { get; set; }

		[DefaultValue(null)]
		[StiSerializable]
		public static string DesignerTitle { get; set; }

		[DefaultValue(null)]
		[StiSerializable]
		public static string DesignerTitleText { get; set; }

		[DefaultValue("{0} - {1}")]
		[StiSerializable]
		public static string DesignerTitleMask { get; set; } = "{0} - {1}";


		[DefaultValue(true)]
		[StiSerializable]
		public static bool ShowRibbonWindowTitleInWpf { get; set; } = true;


		public static Icon DesignerIcon { get; set; }

		[DefaultValue(/*Could not decode attribute arguments.*/)]
		[Description("Gets or sets window state of the Designer window.")]
		[StiSerializable]
		public static FormWindowState DesignerWindowState { get; set; } = (FormWindowState)2;


		public static Size? DesignerWindowSize { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value indicating whether Designer will be shown in the task bar.")]
		[StiSerializable]
		public static bool ShowDesignerInTaskbar { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value indicating whether Designer will be shown in the styles mode.")]
		[StiSerializable]
		public static bool ShowDesignerInStylesMode { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowUseHighlightWizard { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowUsePlacementWizard { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value indicating whether it is necessary to run the Wizard after loading the designer of the report.")]
		[StiSerializable]
		public static bool RunWizardAfterLoad { get; set; }

		[DefaultValue(false)]
		[Description("A string representation of the wizard type which should be run after designer starts.")]
		[StiSerializable]
		public static string RunSpecificWizardAfterLoad { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowRunDataBandPlacementWizard { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool UseComponentPlacementOptimization { get; set; } = true;


		public static event StiGetCulturesEventHandler GetCulturesListForGlobalizationEditor;

		[Obsolete("Please dont use event StiOptions.Designer.CheckDataBandNameCreatedInWizard! You need use event StiOptions.Engine.GlobalEvents.CheckDataBandNameCreatedInWizard instead it.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static event StiCheckDataBandNameCreatedInWizardHandler CheckDataBandNameCreatedInWizard
		{
			add
			{
				Engine.GlobalEvents.CheckDataBandNameCreatedInWizard += value;
			}
			remove
			{
				Engine.GlobalEvents.CheckDataBandNameCreatedInWizard -= value;
			}
		}

		public static CultureInfo[] InvokeGetCulturesListForGlobalizationEditor()
		{
			if (Designer.GetCulturesListForGlobalizationEditor == null)
			{
				return null;
			}
			StiGetCulturesEventArgs stiGetCulturesEventArgs = new StiGetCulturesEventArgs();
			Designer.GetCulturesListForGlobalizationEditor(null, stiGetCulturesEventArgs);
			return stiGetCulturesEventArgs.Cultures;
		}

		[Obsolete("Please dont use method StiOptions.Designer.InvokeCheckDataBandNameCreatedInWizard! You need use method StiOptions.Engine.GlobalEvents.InvokeCheckDataBandNameCreatedInWizard instead it.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void InvokeCheckDataBandNameCreatedInWizard(StiCheckDataBandNameCreatedInWizardArgs checkArgs)
		{
			Engine.GlobalEvents.InvokeCheckDataBandNameCreatedInWizard(checkArgs);
		}

		public static string GetDesignerTitle()
		{
			return DesignerTitle ?? StiLocalization.Get("FormDesigner", "title");
		}

		public static string GetDesignerTitleWithMask(string file)
		{
			if (DesignerTitleText != null)
			{
				return DesignerTitleText;
			}
			if (!string.IsNullOrEmpty(file))
			{
				return string.Format(DesignerTitleMask, Path.GetFileNameWithoutExtension(file), GetDesignerTitle());
			}
			return GetDesignerTitle();
		}
	}

	public sealed class Dictionary
	{
		public sealed class BusinessObjects
		{
			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowUseDataColumn { get; set; } = true;


			[DefaultValue(false)]
			[StiSerializable]
			public static bool AllowUseFields { get; set; }

			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowUseProperties { get; set; } = true;


			[DefaultValue(StiPropertiesProcessingType.Browsable)]
			[Description("Gets or sets a value, which controls processing of properties in business objects.")]
			[StiSerializable]
			public static StiPropertiesProcessingType PropertiesProcessingType { get; set; } = StiPropertiesProcessingType.Browsable;


			[DefaultValue(StiFieldsProcessingType.Browsable)]
			[Description("Gets or sets a value, which controls processing of fields in business objects.")]
			[StiSerializable]
			public static StiFieldsProcessingType FieldsProcessingType { get; set; } = StiFieldsProcessingType.Browsable;


			[DefaultValue('_')]
			[Description("Gets or sets a delimiter between a name of the enumerable data source and its column names for the child tables in the master-detail relation.")]
			[StiSerializable]
			public static char Delimeter { get; set; } = '_';


			[DefaultValue(10)]
			[Description("Gets or sets the maximal nested level of master-detail relations in enumerable sources.")]
			[StiSerializable]
			public static int MaxLevel { get; set; } = 10;


			[DefaultValue(true)]
			[Description("Gets or sets a value, which controls that enumerable sources with the same names.")]
			[StiSerializable]
			public static bool CheckTableDuplication { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool AddBusinessObjectAssemblyToReferencedAssembliesAutomatically { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowProcessNullItemsInEnumerables { get; set; } = true;


			[DefaultValue(StiColumnsSynchronizationMode.KeepAbsentColumns)]
			[StiSerializable]
			public static StiColumnsSynchronizationMode ColumnsSynchronizationMode { get; set; } = StiColumnsSynchronizationMode.KeepAbsentColumns;

		}

		public sealed class Connections
		{
			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowCsv
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiCsvAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiCsvAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiCsvDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowDb2
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiDB2AdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiDB2AdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiDB2Database).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowDBase
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiDBaseAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiDBaseAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiDBaseDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowDotConnectUniversal
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiDotConnectUniversalAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiDotConnectUniversalAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiDotConnectUniversalDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowFirebird
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiFirebirdAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiFirebirdAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiFirebirdDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowInformix
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiInformixAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiInformixAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiInformixDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowMySql
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiMySqlAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiMySqlAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiMySqlDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowMongoDb
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiMongoDbAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiMongoDbAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiMongoDbDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowMsAccess
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiMSAccessAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiMSAccessAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiMSAccessDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowOdbc
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiOdbcAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiOdbcAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiOdbcDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowOleDb
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiOleDbAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiOleDbAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiOleDbDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowOracle
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiOracleAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiOracleAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiOracleDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowPostgreSql
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiPostgreSQLAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiPostgreSQLAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiPostgreSQLDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowSqLite
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiSQLiteAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiSQLiteAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiSQLiteDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowMsSql
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && ((object)s).GetType() == typeof(StiSqlAdapterService));
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => ((object)s).GetType() == typeof(StiSqlAdapterService)).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => ((object)s).GetType() == typeof(StiSqlDatabase)).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowSqlCe
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiSqlCeAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiSqlCeAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiSqlCeDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowSybase
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiSybaseAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiSybaseAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiSybaseDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowSybaseAds
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiSybaseAdsAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiSybaseAdsAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiSybaseAdsDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowTeradata
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiTeradataAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiTeradataAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiTeradataDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowVistaDB
			{
				get
				{
					return Services.DataAdapters.Any((StiDataAdapterService s) => ((StiService)s).ServiceEnabled && s is StiVistaDBAdapterService);
				}
				set
				{
					Services.DataAdapters.Where((StiDataAdapterService s) => s is StiVistaDBAdapterService).ToList().ForEach(delegate(StiDataAdapterService s)
					{
						((StiService)s).ServiceEnabled = value;
					});
					Services.Databases.Where((StiDatabase s) => s is StiVistaDBDatabase).ToList().ForEach(delegate(StiDatabase s)
					{
						((StiService)s).ServiceEnabled = value;
					});
				}
			}
		}

		public sealed class ConvertNulls
		{
			public static DateTime DefaultDateTimeForNullValues
			{
				get
				{
					return StiConvert.DefaultDateTimeForNullValues;
				}
				set
				{
					StiConvert.DefaultDateTimeForNullValues = value;
				}
			}

			[DefaultValue(false)]
			[StiSerializable]
			public static bool UseDefaultDateTimeForNullValues
			{
				get
				{
					return StiConvert.UseDefaultDateTimeForNullValues;
				}
				set
				{
					StiConvert.UseDefaultDateTimeForNullValues = value;
				}
			}
		}

		public sealed class DataAdapters
		{
			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadSybaseSAAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadSybaseSAAdapter;

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadOracleClientAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadOracleClientAdapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadOracleODPAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadOracleODPAdapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadOracleODPManagedAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadOracleODPManagedAdapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadFirebirdAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadFirebirdAdapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadMySqlAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadMySqlAdapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadDotConnectUniversalAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadDotConnectUniversalAdapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadDotConnectOracleAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadDotConnectOracleAdapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadPostgreSQLAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadPostgreSQLAdapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadVistaDBAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadVistaDBAdapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadSqlCeAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadSqlCeAdapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadSQLiteAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadSQLiteAdapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadDB2Adapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadDB2Adapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadUniDirectAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadUniDirectAdapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadSybaseADSAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadSybaseADSAdapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadSybaseASEAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadSybaseASEAdapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadInformixAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadInformixAdapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadEffiProzAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadEffiProzAdapter { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Dictionary.DataAdapters.TryToLoadTeradataAdapter is obsolete. Starts from version 2015.2 data adapters fully integrated to the report engine. So you need only copy a .Net data provider assemblies to your application folder.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool TryToLoadTeradataAdapter { get; set; }
		}

		public sealed class Enumerables
		{
			[Obsolete("Please use property StiOptions.Dictionary.BusinessObjects.PropertiesProcessingType")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static StiPropertiesProcessingType PropertiesProcessingType
			{
				get
				{
					return BusinessObjects.PropertiesProcessingType;
				}
				set
				{
					BusinessObjects.PropertiesProcessingType = value;
				}
			}

			[Obsolete("Please use property StiOptions.Dictionary.BusinessObjects.Delimeter")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static char Delimeter
			{
				get
				{
					return BusinessObjects.Delimeter;
				}
				set
				{
					BusinessObjects.Delimeter = value;
				}
			}

			[Obsolete("Please use property StiOptions.Dictionary.BusinessObjects.MaxLevel")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static int MaxLevel
			{
				get
				{
					return BusinessObjects.MaxLevel;
				}
				set
				{
					BusinessObjects.MaxLevel = value;
				}
			}

			[Obsolete("Please use property StiOptions.Dictionary.BusinessObjects.CheckTableDuplication")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool CheckTableDuplication
			{
				get
				{
					return BusinessObjects.CheckTableDuplication;
				}
				set
				{
					BusinessObjects.CheckTableDuplication = value;
				}
			}
		}

		public sealed class Json
		{
			[DefaultValue(false)]
			[StiSerializable]
			public static bool ParseCreatingTableProperties
			{
				get
				{
					return StiBaseOptions.JsonParseCreatingTableProperties;
				}
				set
				{
					StiBaseOptions.JsonParseCreatingTableProperties = value;
				}
			}

			[DefaultValue(false)]
			[StiSerializable]
			public static bool ParseTypeAsString
			{
				get
				{
					return StiBaseOptions.JsonParseTypeAsString;
				}
				set
				{
					StiBaseOptions.JsonParseTypeAsString = value;
				}
			}

			[DefaultValue(/*Could not decode attribute arguments.*/)]
			[StiSerializable]
			public static StiJsonConverterVersion DefaultJsonConverterVersion
			{
				get
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					return StiBaseOptions.DefaultJsonConverterVersion;
				}
				set
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					StiBaseOptions.DefaultJsonConverterVersion = value;
				}
			}
		}

		[DefaultValue(StiAutoSynchronizeMode.Always)]
		[Description("Gets or sets a value which controls synchronization of the Dictionary in automatic mode.")]
		[StiSerializable]
		public static StiAutoSynchronizeMode AutoSynchronize { get; set; } = StiAutoSynchronizeMode.Always;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool RemoveUnusedDataBeforeReportRendering { get; set; }

		public static Type DefaultDataAdapterType { get; set; }

		[DefaultValue(30)]
		[StiSerializable]
		public static int QueryBuilderConnectTimeout { get; set; } = 30;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool UseAdvancedDataSearch { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowConnectToFirstTableForEmptyDataSource { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value which indicates that ConnectOnStart property will turn on second pass of report rendering automatically.")]
		[StiSerializable]
		public static bool EnableConnectOnStartOnSecondPass { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value indicating that instead of a component name an alias will be shown.")]
		[StiSerializable]
		public static bool ShowOnlyAliasForComponents { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value indicating that instead of a page name an alias will be shown.")]
		[StiSerializable]
		public static bool ShowOnlyAliasForPages { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value indicating that instead of a database name an alias will be shown.")]
		[StiSerializable]
		public static bool ShowOnlyAliasForDatabase { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value indicating that instead of a data name an alias will be shown.")]
		[StiSerializable]
		public static bool ShowOnlyAliasForData { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value indicating that instead of a variable name an alias will be shown.")]
		[StiSerializable]
		public static bool ShowOnlyAliasForVariable { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value indicating that instead of a resource name an alias will be shown.")]
		[StiSerializable]
		public static bool ShowOnlyAliasForResource { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value indicating that instead of a total name an alias will be shown.")]
		[StiSerializable]
		[Obsolete("Property 'StiOptions.Dictionary.ShowOnlyAliasForTotal' is not used more!")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool ShowOnlyAliasForTotal { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value indicating that instead of a DataSource name an alias will be shown.")]
		[StiSerializable]
		public static bool ShowOnlyAliasForDataSource { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value indicating that instead of a business object name an alias will be shown.")]
		[StiSerializable]
		public static bool ShowOnlyAliasForBusinessObject { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value indicating that instead of a DataColumn name an alias will be shown.")]
		[StiSerializable]
		public static bool ShowOnlyAliasForDataColumn { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value indicating that instead of a DataRelation name an alias will be shown.")]
		[StiSerializable]
		public static bool ShowOnlyAliasForDataRelation { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool HideRelationExceptions { get; set; } = true;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool UseNullableDateTime { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		public static bool UseNullableTimeSpan { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		public static bool NotIdenticalNameAndAliasAtRegistrationOfNewData { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		public static bool ReplaceExistingDataAtRegistrationOfNewData { get; set; } = true;


		[DefaultValue(StiColumnsSynchronizationMode.KeepAbsentColumns)]
		[StiSerializable]
		public static StiColumnsSynchronizationMode ColumnsSynchronizationMode { get; set; } = StiColumnsSynchronizationMode.KeepAbsentColumns;


		[DefaultValue(StiColumnsSynchronizationMode.KeepAbsentColumns)]
		[StiSerializable]
		public static StiColumnTypeSynchronizationMode ColumnTypeSynchronizationMode { get; set; } = StiColumnTypeSynchronizationMode.KeepAsIs;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool UseAllowDBNullProperty { get; set; }
	}

	public sealed class Engine
	{
		public sealed class Blockly
		{
			[DefaultValue(30)]
			[StiSerializable]
			public static int ExecutionTimeout { get; set; } = 30;

		}

		public sealed class CrossTab
		{
			[DefaultValue(5.0)]
			[StiSerializable]
			public static double DefaultWidth { get; set; } = 5.0;


			[DefaultValue(5.0)]
			[StiSerializable]
			public static double DefaultHeight { get; set; } = 5.0;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool MemoryOptimization { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets value which indicates that report engine can start the Garbage Collector during rendering.")]
			[StiSerializable]
			public static bool AllowGCCollect { get; set; } = true;

		}

		public sealed class GlobalEvents
		{
			public class StiImportXmlSchemaEventArg : EventArgs
			{
				public DataSet DataSet { get; }

				public StiImportXmlSchemaEventArg(DataSet dataSet)
				{
					DataSet = dataSet;
				}
			}

			public delegate void StiImportXmlSchemaEventHandler(object sender, StiImportXmlSchemaEventArg e);

			[CompilerGenerated]
			private static DragEventHandler m_DragDropToPageInDesigner;

			[CompilerGenerated]
			private static DragEventHandler m_DragEnterToPageInDesigner;

			[CompilerGenerated]
			private static DragEventHandler m_DragOverPageInDesigner;

			[CompilerGenerated]
			private static QueryContinueDragEventHandler m_QueryContinueDragOverPageInDesigner;

			[CompilerGenerated]
			private static MouseEventHandler m_MouseClick;

			public static bool AllowDropFileInViewer => GlobalEvents.DropFileInViewer != null;

			public static bool AllowPasteDataInViewer => GlobalEvents.PasteDataInViewer != null;

			public static event CustomHelpEventHandler OpenHelp;

			public static event StiGetValueEventHandler GetValue;

			public static event EventHandler DataSourceAssigned;

			public static event EventHandler BusinessObjectAssigned;

			public static event EventHandler BeforePrint;

			public static event EventHandler AfterPrint;

			public static event StiTextChangedEventHandler TextChanged;

			public static event StiComponentCreationHandler ComponentCreated;

			public static event StiComponentRemovingHandler ComponentRemoved;

			public static event StiProcessExportEventHandler ProcessExport;

			public static event EventHandler GlobalGuiStyleChanged;

			public static event StiAutoLargeHeightEventHandler AutoLargeHeightChanged;

			public static event StiGetPropertiesEventHandler GetProperties;

			public static event StiAllowClipboardOperationEventHandler AllowCutOperation;

			public static event StiAllowClipboardOperationEventHandler AllowCopyOperation;

			public static event StiAllowClipboardOperationEventHandler AllowPasteOperation;

			public static event StiPastedEventHandler Pasted;

			public static event StiImportXmlSchemaEventHandler ImportXmlSchema;

			public static event StiSavingStylesEventHandler SavingStylesInDesigner;

			public static event StiLoadingStylesEventHandler LoadingStylesInDesigner;

			public static event DragEventHandler DragDropToPageInDesigner
			{
				[CompilerGenerated]
				add
				{
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0015: Expected O, but got Unknown
					DragEventHandler val = GlobalEvents.m_DragDropToPageInDesigner;
					DragEventHandler val2;
					do
					{
						val2 = val;
						DragEventHandler value2 = (DragEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
						val = Interlocked.CompareExchange(ref GlobalEvents.m_DragDropToPageInDesigner, value2, val2);
					}
					while (val != val2);
				}
				[CompilerGenerated]
				remove
				{
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0015: Expected O, but got Unknown
					DragEventHandler val = GlobalEvents.m_DragDropToPageInDesigner;
					DragEventHandler val2;
					do
					{
						val2 = val;
						DragEventHandler value2 = (DragEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
						val = Interlocked.CompareExchange(ref GlobalEvents.m_DragDropToPageInDesigner, value2, val2);
					}
					while (val != val2);
				}
			}

			public static event DragEventHandler DragEnterToPageInDesigner
			{
				[CompilerGenerated]
				add
				{
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0015: Expected O, but got Unknown
					DragEventHandler val = GlobalEvents.m_DragEnterToPageInDesigner;
					DragEventHandler val2;
					do
					{
						val2 = val;
						DragEventHandler value2 = (DragEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
						val = Interlocked.CompareExchange(ref GlobalEvents.m_DragEnterToPageInDesigner, value2, val2);
					}
					while (val != val2);
				}
				[CompilerGenerated]
				remove
				{
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0015: Expected O, but got Unknown
					DragEventHandler val = GlobalEvents.m_DragEnterToPageInDesigner;
					DragEventHandler val2;
					do
					{
						val2 = val;
						DragEventHandler value2 = (DragEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
						val = Interlocked.CompareExchange(ref GlobalEvents.m_DragEnterToPageInDesigner, value2, val2);
					}
					while (val != val2);
				}
			}

			public static event EventHandler DragLeaveFromPageInDesigner;

			public static event DragEventHandler DragOverPageInDesigner
			{
				[CompilerGenerated]
				add
				{
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0015: Expected O, but got Unknown
					DragEventHandler val = GlobalEvents.m_DragOverPageInDesigner;
					DragEventHandler val2;
					do
					{
						val2 = val;
						DragEventHandler value2 = (DragEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
						val = Interlocked.CompareExchange(ref GlobalEvents.m_DragOverPageInDesigner, value2, val2);
					}
					while (val != val2);
				}
				[CompilerGenerated]
				remove
				{
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0015: Expected O, but got Unknown
					DragEventHandler val = GlobalEvents.m_DragOverPageInDesigner;
					DragEventHandler val2;
					do
					{
						val2 = val;
						DragEventHandler value2 = (DragEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
						val = Interlocked.CompareExchange(ref GlobalEvents.m_DragOverPageInDesigner, value2, val2);
					}
					while (val != val2);
				}
			}

			public static event QueryContinueDragEventHandler QueryContinueDragOverPageInDesigner
			{
				[CompilerGenerated]
				add
				{
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0015: Expected O, but got Unknown
					QueryContinueDragEventHandler val = GlobalEvents.m_QueryContinueDragOverPageInDesigner;
					QueryContinueDragEventHandler val2;
					do
					{
						val2 = val;
						QueryContinueDragEventHandler value2 = (QueryContinueDragEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
						val = Interlocked.CompareExchange(ref GlobalEvents.m_QueryContinueDragOverPageInDesigner, value2, val2);
					}
					while (val != val2);
				}
				[CompilerGenerated]
				remove
				{
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0015: Expected O, but got Unknown
					QueryContinueDragEventHandler val = GlobalEvents.m_QueryContinueDragOverPageInDesigner;
					QueryContinueDragEventHandler val2;
					do
					{
						val2 = val;
						QueryContinueDragEventHandler value2 = (QueryContinueDragEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
						val = Interlocked.CompareExchange(ref GlobalEvents.m_QueryContinueDragOverPageInDesigner, value2, val2);
					}
					while (val != val2);
				}
			}

			public static event StiOpenRecentFileObjectEventHandler OpenRecentFileInDesigner;

			public static event StiDisplayRecentFileObjectEventHandler DisplayRecentFileInDesigner;

			public static event EventHandler LoadingDesigner;

			public static event EventHandler LoadedDesigner;

			public static event StiSaveChangesInDialogBoxDesignerEventHandler SaveChangesInDialogBoxDesigner;

			public static event EventHandler LoadingConfigurationInDesigner;

			public static event EventHandler LoadedConfigurationInDesigner;

			public static event EventHandler SavingConfigurationInDesigner;

			public static event EventHandler SavedConfigurationInDesigner;

			public static event CancelEventHandler ClosingDesigner;

			public static event EventHandler ClosedDesigner;

			public static event EventHandler ReportChangedInDesigner;

			public static event EventHandler DictionaryMenuActionsOpenInDesigner;

			public static event EventHandler DictionaryMenuNewOpenInDesigner;

			public static event EventHandler DictionaryMenuEditOpenInDesigner;

			public static event StiSavingObjectEventHandler SavingReportInDesigner;

			public static event EventHandler SavedReportInDesigner;

			public static event StiLoadingObjectEventHandler LoadingReportInDesigner;

			public static event EventHandler LoadedReportInDesigner;

			public static event StiCreatingObjectEventHandler CreatingReportInDesigner;

			public static event EventHandler CreatedReportInDesigner;

			public static event EventHandler SavingPageInDesigner;

			public static event EventHandler LoadingPageInDesigner;

			public static event EventHandler PreviewingReportInDesigner;

			public static event DragDropFromDictionaryEventHandler DragDropFromDictionary;

			public static event EventHandler WizardStart;

			public static event EventHandler WizardFinish;

			public static event EventHandler RunPageSetupInDesigner;

			public static event StiCheckDataBandNameCreatedInWizardHandler CheckDataBandNameCreatedInWizard;

			public static event EventHandler ReportNameChanged;

			public static event EventHandler ReportCacheCreated;

			public static event EventHandler ReportSynchronized;

			public static event EventHandler ReportExporting;

			public static event EventHandler ReportExported;

			public static event EventHandler ReportPrinting;

			public static event EventHandler ReportPrinted;

			public static event EventHandler ReportPrintingPage;

			public static event EventHandler ReportPrintedPage;

			public static event EventHandler ReportRefreshing;

			public static event StiGetSubReportEventHandler ReportGetSubReport;

			public static event EventHandler ReportLoading;

			public static event EventHandler ReportLoaded;

			public static event EventHandler ReportSaving;

			public static event EventHandler ReportSaved;

			public static event EventHandler ReportCompiling;

			public static event EventHandler ReportCompiled;

			public static event EventHandler ReportBeginRender;

			public static event EventHandler ReportRendering;

			public static event EventHandler ReportEndRender;

			public static event EventHandler ReportDisposed;

			public static event StiSendEMailEventHandler SendEMailProcess;

			public static event StiSaveDocumentEventHandler SaveDocumentProcess;

			public static event EventHandler LoadViewerControl;

			public static event StiDropFileInViewerEventHandler DropFileInViewer;

			public static event StiPasteDataInViewerEventHandler PasteDataInViewer;

			public static event EventHandler SavingDocumentInViewer;

			public static event EventHandler LoadingDocumentInViewer;

			public static event EventHandler DocumentLoadedInViewer;

			public static event EventHandler PrintingDocumentInViewer;

			public static event EventHandler StartEditingInPreview;

			public static event EventHandler EndEditingInPreview;

			public static event EventHandler EnableEditorMode;

			public static event EventHandler DisableEditorMode;

			public static event EventHandler Click;

			public static event MouseEventHandler MouseClick
			{
				[CompilerGenerated]
				add
				{
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0015: Expected O, but got Unknown
					MouseEventHandler val = GlobalEvents.m_MouseClick;
					MouseEventHandler val2;
					do
					{
						val2 = val;
						MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
						val = Interlocked.CompareExchange(ref GlobalEvents.m_MouseClick, value2, val2);
					}
					while (val != val2);
				}
				[CompilerGenerated]
				remove
				{
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0015: Expected O, but got Unknown
					MouseEventHandler val = GlobalEvents.m_MouseClick;
					MouseEventHandler val2;
					do
					{
						val2 = val;
						MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
						val = Interlocked.CompareExchange(ref GlobalEvents.m_MouseClick, value2, val2);
					}
					while (val != val2);
				}
			}

			public static event EventHandler DoubleClick;

			public static event StiHyperlinkClickEventHandlers HyperlinkClick;

			public static event StiCheckDrillDownReportEventHandler CheckDrillDownReport;

			public static event StiGetDrillDownReportEventHandler GetDrillDownReport;

			public static event StiNewRequestFromUserDialogCreatingEventHandler NewRequestFromUserDialogCreating;

			public static bool InvokeOpenCustomHelp(StiHelpViewerForm helpViewerForm)
			{
				if (GlobalEvents.OpenHelp == null)
				{
					return false;
				}
				CustomHelpEventArgs customHelpEventArgs = new CustomHelpEventArgs();
				GlobalEvents.OpenHelp(helpViewerForm, customHelpEventArgs);
				if (!customHelpEventArgs.Processed)
				{
					return customHelpEventArgs.Processed;
				}
				StiHelpViewerForm.Url = customHelpEventArgs.Url;
				((Control)helpViewerForm).Text = customHelpEventArgs.HeaderText;
				return customHelpEventArgs.Processed;
			}

			public static void InvokeGetValue(object sender, StiGetValueEventArgs e)
			{
				GlobalEvents.GetValue?.Invoke(sender, e);
			}

			public static void InvokeDataSourceAssigned(object sender, EventArgs e)
			{
				GlobalEvents.DataSourceAssigned?.Invoke(sender, e);
			}

			public static void InvokeBusinessObjectAssigned(object sender, EventArgs e)
			{
				GlobalEvents.BusinessObjectAssigned?.Invoke(sender, e);
			}

			public static void InvokeBeforePrint(object sender, EventArgs e)
			{
				GlobalEvents.BeforePrint?.Invoke(sender, e);
			}

			public static void InvokeAfterPrint(object sender, EventArgs e)
			{
				GlobalEvents.AfterPrint?.Invoke(sender, e);
			}

			public static void InvokeTextChanged(object sender, StiTextChangedEventArgs e)
			{
				GlobalEvents.TextChanged?.Invoke(sender, e);
			}

			public static void InvokeComponentCreated(object sender, StiComponentCreationEventArgs e)
			{
				GlobalEvents.ComponentCreated?.Invoke(sender, e);
			}

			public static void InvokeComponentRemoved(object sender, StiComponentRemovingEventArgs e)
			{
				GlobalEvents.ComponentRemoved?.Invoke(sender, e);
			}

			public static void InvokeProcessExport(object sender, StiProcessExportEventArgs e)
			{
				GlobalEvents.ProcessExport?.Invoke(sender, e);
			}

			private static void InvokeGlobalGuiStyleChanged(Control parent)
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Expected O, but got Unknown
				if (parent is IStiApplyStyle stiApplyStyle)
				{
					stiApplyStyle.ApplyStyle();
				}
				foreach (Control item in (ArrangedElementCollection)parent.Controls)
				{
					Control parent2 = item;
					InvokeGlobalGuiStyleChanged(parent2);
				}
			}

			public static void InvokeGlobalGuiStyleChanged(object sender, EventArgs e)
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Expected O, but got Unknown
				foreach (Form item in (ReadOnlyCollectionBase)(object)Application.OpenForms)
				{
					Form parent = item;
					InvokeGlobalGuiStyleChanged((Control)(object)parent);
				}
				GlobalEvents.GlobalGuiStyleChanged?.Invoke(sender, e);
			}

			public static void InvokeAutoLargeHeightChanged(object sender, StiAutoLargeHeightEventArgs e)
			{
				GlobalEvents.AutoLargeHeightChanged?.Invoke(sender, e);
			}

			public static void InvokeGetProperties(object sender, StiGetPropertiesEventArgs e)
			{
				GlobalEvents.GetProperties?.Invoke(sender, e);
			}

			public static void InvokeAllowCutOperation(object sender, StiAllowClipboardOperationEventArgs e)
			{
				if (GlobalEvents.AllowCutOperation != null)
				{
					GlobalEvents.AllowCutOperation(sender, e);
				}
			}

			public static void InvokeAllowCopyOperation(object sender, StiAllowClipboardOperationEventArgs e)
			{
				if (GlobalEvents.AllowCopyOperation != null)
				{
					GlobalEvents.AllowCopyOperation(sender, e);
				}
			}

			public static void InvokeAllowPasteOperation(object sender, StiAllowClipboardOperationEventArgs e)
			{
				if (GlobalEvents.AllowPasteOperation != null)
				{
					GlobalEvents.AllowPasteOperation(sender, e);
				}
			}

			public static void InvokePasted(object sender, StiPastedEventArgs e)
			{
				if (GlobalEvents.Pasted != null)
				{
					GlobalEvents.Pasted(sender, e);
				}
			}

			public static void InvokeImportXmlSchema(object sender, StiImportXmlSchemaEventArg e)
			{
				if (GlobalEvents.ImportXmlSchema != null)
				{
					GlobalEvents.ImportXmlSchema(sender, e);
				}
			}

			public static bool InvokeSavingStylesInDesigner(IStiDesignerBase designer, StiStylesCollection styles)
			{
				if (GlobalEvents.SavingStylesInDesigner != null)
				{
					StiSavingStylesEventArgs stiSavingStylesEventArgs = new StiSavingStylesEventArgs(styles);
					GlobalEvents.SavingStylesInDesigner(designer, stiSavingStylesEventArgs);
					return stiSavingStylesEventArgs.Processed;
				}
				return false;
			}

			public static bool InvokeLoadingStylesInDesigner(IStiDesignerBase designer, StiStylesCollection styles)
			{
				if (GlobalEvents.LoadingStylesInDesigner != null)
				{
					StiLoadingStylesEventArgs stiLoadingStylesEventArgs = new StiLoadingStylesEventArgs(styles);
					GlobalEvents.LoadingStylesInDesigner(designer, stiLoadingStylesEventArgs);
					return stiLoadingStylesEventArgs.Processed;
				}
				return false;
			}

			public static void InvokeDragDropToPageInDesigner(IStiDesignerBase designer, DragEventArgs e)
			{
				if (GlobalEvents.DragDropToPageInDesigner != null)
				{
					GlobalEvents.DragDropToPageInDesigner.Invoke((object)designer, e);
				}
			}

			public static void InvokeDragEnterToPageInDesigner(IStiDesignerBase designer, DragEventArgs e)
			{
				if (GlobalEvents.DragEnterToPageInDesigner != null)
				{
					GlobalEvents.DragEnterToPageInDesigner.Invoke((object)designer, e);
				}
			}

			public static void InvokeDragLeaveFromPageInDesigner(IStiDesignerBase designer, EventArgs e)
			{
				if (GlobalEvents.DragLeaveFromPageInDesigner != null)
				{
					GlobalEvents.DragLeaveFromPageInDesigner(designer, e);
				}
			}

			public static void InvokeDragOverPageInDesigner(IStiDesignerBase designer, DragEventArgs e)
			{
				if (GlobalEvents.DragOverPageInDesigner != null)
				{
					GlobalEvents.DragOverPageInDesigner.Invoke((object)designer, e);
				}
			}

			public static void InvokeQueryContinueDragOverPageInDesigner(IStiDesignerBase designer, QueryContinueDragEventArgs e)
			{
				if (GlobalEvents.QueryContinueDragOverPageInDesigner != null)
				{
					GlobalEvents.QueryContinueDragOverPageInDesigner.Invoke((object)designer, e);
				}
			}

			public static bool InvokeOpenRecentFileInDesigner(IStiDesignerBase designer, StiRecentFile recentFile)
			{
				if (GlobalEvents.OpenRecentFileInDesigner != null)
				{
					StiOpenRecentFileObjectEventArgs stiOpenRecentFileObjectEventArgs = new StiOpenRecentFileObjectEventArgs(recentFile);
					GlobalEvents.OpenRecentFileInDesigner(designer, stiOpenRecentFileObjectEventArgs);
					return stiOpenRecentFileObjectEventArgs.Processed;
				}
				return false;
			}

			public static StiDisplayRecentFileObjectEventArgs InvokeDisplayRecentFileInDesigner(IStiDesignerBase designer, StiRecentFile recentFile)
			{
				StiDisplayRecentFileObjectEventArgs stiDisplayRecentFileObjectEventArgs = new StiDisplayRecentFileObjectEventArgs(recentFile);
				if (GlobalEvents.DisplayRecentFileInDesigner != null)
				{
					GlobalEvents.DisplayRecentFileInDesigner(designer, stiDisplayRecentFileObjectEventArgs);
				}
				return stiDisplayRecentFileObjectEventArgs;
			}

			public static void InvokeLoadingDesigner(IStiDesignerBase designer, EventArgs e)
			{
				if (GlobalEvents.LoadingDesigner != null)
				{
					GlobalEvents.LoadingDesigner(designer, e);
				}
			}

			public static void InvokeLoadedDesigner(IStiDesignerBase designer, EventArgs e)
			{
				if (GlobalEvents.LoadedDesigner != null)
				{
					GlobalEvents.LoadedDesigner(designer, e);
				}
			}

			public static void InvokeSaveChangesInDialogBoxDesigner(IStiDesignerBase designer, StiSaveChangesInDialogBoxDesignerEvenArgs e)
			{
				if (GlobalEvents.SaveChangesInDialogBoxDesigner != null)
				{
					GlobalEvents.SaveChangesInDialogBoxDesigner(designer, e);
				}
			}

			public static void InvokeLoadingConfigurationInDesigner(IStiDesignerBase designer, EventArgs e)
			{
				if (GlobalEvents.LoadingConfigurationInDesigner != null)
				{
					GlobalEvents.LoadingConfigurationInDesigner(designer, e);
				}
			}

			public static void InvokeLoadedConfigurationInDesigner(IStiDesignerBase designer, EventArgs e)
			{
				if (GlobalEvents.LoadedConfigurationInDesigner != null)
				{
					GlobalEvents.LoadedConfigurationInDesigner(designer, e);
				}
			}

			public static void InvokeSavingConfigurationInDesigner(IStiDesignerBase designer, EventArgs e)
			{
				if (GlobalEvents.SavingConfigurationInDesigner != null)
				{
					GlobalEvents.SavingConfigurationInDesigner(designer, e);
				}
			}

			public static void InvokeSavedConfigurationInDesigner(IStiDesignerBase designer, EventArgs e)
			{
				if (GlobalEvents.SavedConfigurationInDesigner != null)
				{
					GlobalEvents.SavedConfigurationInDesigner(designer, e);
				}
			}

			public static void InvokeClosingDesigner(IStiDesignerBase designer, CancelEventArgs e)
			{
				if (GlobalEvents.ClosingDesigner != null)
				{
					GlobalEvents.ClosingDesigner(designer, e);
				}
			}

			public static void InvokeClosedDesigner(IStiDesignerBase designer, EventArgs e)
			{
				if (GlobalEvents.ClosedDesigner != null)
				{
					GlobalEvents.ClosedDesigner(designer, e);
				}
			}

			public static void InvokeReportChangedInDesigner(IStiDesignerBase designer, EventArgs e)
			{
				if (GlobalEvents.ReportChangedInDesigner != null)
				{
					GlobalEvents.ReportChangedInDesigner(designer, e);
				}
			}

			public static void InvokeDictionaryMenuActionsOpenInDesigner(object sender)
			{
				if (GlobalEvents.DictionaryMenuActionsOpenInDesigner != null)
				{
					GlobalEvents.DictionaryMenuActionsOpenInDesigner(sender, EventArgs.Empty);
				}
			}

			public static void InvokeDictionaryMenuNewOpenInDesigner(object sender)
			{
				if (GlobalEvents.DictionaryMenuNewOpenInDesigner != null)
				{
					GlobalEvents.DictionaryMenuNewOpenInDesigner(sender, EventArgs.Empty);
				}
			}

			public static void InvokeDictionaryMenuEditOpenInDesigner(object sender)
			{
				if (GlobalEvents.DictionaryMenuEditOpenInDesigner != null)
				{
					GlobalEvents.DictionaryMenuEditOpenInDesigner(sender, EventArgs.Empty);
				}
			}

			public static bool InvokeSavingReportInDesigner(IStiDesignerBase designer, bool saveAs)
			{
				return InvokeSavingReportInDesigner(designer, saveAs, StiSaveEventSource.Underfined);
			}

			public static bool InvokeSavingReportInDesigner(IStiDesignerBase designer, bool saveAs, StiSaveEventSource eventSource)
			{
				if (GlobalEvents.SavingReportInDesigner != null)
				{
					StiSavingObjectEventArgs stiSavingObjectEventArgs = new StiSavingObjectEventArgs(saveAs, eventSource);
					GlobalEvents.SavingReportInDesigner(designer, stiSavingObjectEventArgs);
					return stiSavingObjectEventArgs.Processed;
				}
				return false;
			}

			public static void InvokeSavedReportInDesigner(IStiDesignerBase designer)
			{
				if (GlobalEvents.SavedReportInDesigner != null)
				{
					GlobalEvents.SavedReportInDesigner(designer, EventArgs.Empty);
				}
			}

			public static bool InvokeLoadingReportInDesigner(IStiDesignerBase designer)
			{
				if (GlobalEvents.LoadingReportInDesigner != null)
				{
					StiLoadingObjectEventArgs stiLoadingObjectEventArgs = new StiLoadingObjectEventArgs();
					GlobalEvents.LoadingReportInDesigner(designer, stiLoadingObjectEventArgs);
					return stiLoadingObjectEventArgs.Processed;
				}
				return false;
			}

			public static void InvokeLoadedReportInDesigner(IStiDesignerBase designer)
			{
				if (GlobalEvents.LoadedReportInDesigner != null)
				{
					GlobalEvents.LoadedReportInDesigner(designer, EventArgs.Empty);
				}
			}

			public static bool InvokeCreatingReportInDesigner(IStiDesignerBase designer)
			{
				if (GlobalEvents.CreatingReportInDesigner != null)
				{
					StiCreatingObjectEventArgs stiCreatingObjectEventArgs = new StiCreatingObjectEventArgs();
					GlobalEvents.CreatingReportInDesigner(designer, stiCreatingObjectEventArgs);
					return stiCreatingObjectEventArgs.Processed;
				}
				return false;
			}

			public static void InvokeCreatedReportInDesigner(IStiDesignerBase designer)
			{
				if (GlobalEvents.CreatedReportInDesigner != null)
				{
					GlobalEvents.CreatedReportInDesigner(designer, EventArgs.Empty);
				}
			}

			public static bool InvokeSavingPageInDesigner(IStiDesignerBase designer)
			{
				if (GlobalEvents.SavingPageInDesigner != null)
				{
					GlobalEvents.SavingPageInDesigner(designer, EventArgs.Empty);
					return true;
				}
				return false;
			}

			public static bool InvokeLoadingPageInDesigner(IStiDesignerBase designer)
			{
				if (GlobalEvents.LoadingPageInDesigner != null)
				{
					GlobalEvents.LoadingPageInDesigner(designer, EventArgs.Empty);
					return true;
				}
				return false;
			}

			public static bool InvokePreviewingReportInDesigner(IStiDesignerBase designer)
			{
				if (GlobalEvents.PreviewingReportInDesigner != null)
				{
					GlobalEvents.PreviewingReportInDesigner(designer, EventArgs.Empty);
					return true;
				}
				return false;
			}

			public static void InvokeDragDropFromDictionary(object sender, StiDragDropFromDictionaryEventArgs e)
			{
				if (GlobalEvents.DragDropFromDictionary != null)
				{
					GlobalEvents.DragDropFromDictionary(sender, e);
				}
			}

			public static void InvokeWizardStart(object sender, EventArgs e)
			{
				if (GlobalEvents.WizardStart != null)
				{
					GlobalEvents.WizardStart(sender, e);
				}
			}

			public static void InvokeWizardFinish(object sender, EventArgs e)
			{
				if (GlobalEvents.WizardFinish != null)
				{
					GlobalEvents.WizardFinish(sender, e);
				}
			}

			public static bool InvokeRunPageSetupInDesigner(StiPage page)
			{
				if (GlobalEvents.RunPageSetupInDesigner == null)
				{
					return false;
				}
				GlobalEvents.RunPageSetupInDesigner(page, EventArgs.Empty);
				return true;
			}

			public static void InvokeCheckDataBandNameCreatedInWizard(StiCheckDataBandNameCreatedInWizardArgs checkArgs)
			{
				if (GlobalEvents.CheckDataBandNameCreatedInWizard != null)
				{
					GlobalEvents.CheckDataBandNameCreatedInWizard(null, checkArgs);
				}
			}

			public static void InvokeReportNameChanged(StiReport report, EventArgs e)
			{
				GlobalEvents.ReportNameChanged?.Invoke(report, e);
			}

			public static void InvokeReportCacheCreated(object sender, EventArgs e)
			{
				GlobalEvents.ReportCacheCreated?.Invoke(sender, e);
			}

			public static void InvokeReportSynchronized(object sender, EventArgs e)
			{
				GlobalEvents.ReportSynchronized?.Invoke(sender, e);
			}

			public static void InvokeReportExporting(object sender, StiExportEventArgs e)
			{
				GlobalEvents.ReportExporting?.Invoke(sender, e);
			}

			public static void InvokeReportExported(object sender, StiExportEventArgs e)
			{
				GlobalEvents.ReportExported?.Invoke(sender, e);
			}

			public static void InvokeReportPrinting(object sender, EventArgs e)
			{
				GlobalEvents.ReportPrinting?.Invoke(sender, e);
			}

			public static void InvokeReportPrinted(object sender, EventArgs e)
			{
				GlobalEvents.ReportPrinted?.Invoke(sender, e);
			}

			public static void InvokeReportPrintingPage(object sender, EventArgs e)
			{
				GlobalEvents.ReportPrintingPage?.Invoke(sender, e);
			}

			public static void InvokeReportPrintedPage(object sender, EventArgs e)
			{
				GlobalEvents.ReportPrintedPage?.Invoke(sender, e);
			}

			public static void InvokeReportRefreshing(object sender, EventArgs e)
			{
				GlobalEvents.ReportRefreshing?.Invoke(sender, e);
			}

			public static void InvokeReportGetSubReport(object sender, StiGetSubReportEventArgs e)
			{
				GlobalEvents.ReportGetSubReport?.Invoke(sender, e);
			}

			public static void InvokeReportLoading(object sender, EventArgs e)
			{
				GlobalEvents.ReportLoading?.Invoke(sender, e);
			}

			public static void InvokeReportLoaded(object sender, EventArgs e)
			{
				GlobalEvents.ReportLoaded?.Invoke(sender, e);
			}

			public static void InvokeReportSaving(object sender, EventArgs e)
			{
				GlobalEvents.ReportSaving?.Invoke(sender, e);
			}

			public static void InvokeReportSaved(object sender, EventArgs e)
			{
				GlobalEvents.ReportSaved?.Invoke(sender, e);
			}

			public static void InvokeReportCompiling(object sender, EventArgs e)
			{
				GlobalEvents.ReportCompiling?.Invoke(sender, e);
			}

			public static void InvokeReportCompiled(object sender, EventArgs e)
			{
				GlobalEvents.ReportCompiled?.Invoke(sender, e);
			}

			public static void InvokeReportBeginRender(object sender, EventArgs e)
			{
				GlobalEvents.ReportBeginRender?.Invoke(sender, e);
			}

			public static void InvokeReportRendering(object sender, EventArgs e)
			{
				GlobalEvents.ReportRendering?.Invoke(sender, e);
			}

			public static void InvokeReportEndRender(object sender, EventArgs e)
			{
				GlobalEvents.ReportEndRender?.Invoke(sender, e);
			}

			public static void InvokeReportDisposed(StiReport report, EventArgs e)
			{
				GlobalEvents.ReportDisposed?.Invoke(report, e);
			}

			public static void InvokeSendEMailProcess(object sender, StiSendEMailEventArgs e)
			{
				GlobalEvents.SendEMailProcess?.Invoke(sender, e);
			}

			public static void InvokeSaveDocumentProcess(object sender, StiSaveDocumentEventArgs e)
			{
				GlobalEvents.SaveDocumentProcess?.Invoke(sender, e);
			}

			public static void InvokeLoadViewerControl(object control)
			{
				GlobalEvents.LoadViewerControl?.Invoke(control, EventArgs.Empty);
			}

			public static bool InvokeDropFileInViewer(StiReport report, StiDropFileInViewerEventArgs args)
			{
				if (GlobalEvents.DropFileInViewer != null)
				{
					GlobalEvents.DropFileInViewer(report, args);
					return true;
				}
				return false;
			}

			public static bool InvokePasteDataInViewer(StiReport report, StiPasteDataInViewerEventArgs args)
			{
				if (GlobalEvents.PasteDataInViewer != null)
				{
					GlobalEvents.PasteDataInViewer(report, args);
					return true;
				}
				return false;
			}

			public static bool InvokeSavingDocumentInViewer(StiReport report)
			{
				if (GlobalEvents.SavingDocumentInViewer != null)
				{
					GlobalEvents.SavingDocumentInViewer(report, EventArgs.Empty);
					return true;
				}
				return false;
			}

			public static bool InvokeLoadingDocumentInViewer(StiReport report)
			{
				if (GlobalEvents.LoadingDocumentInViewer != null)
				{
					GlobalEvents.LoadingDocumentInViewer(report, EventArgs.Empty);
					return true;
				}
				return false;
			}

			public static void InvokeDocumentLoadedInViewer(IStiViewerControl viewer)
			{
				GlobalEvents.DocumentLoadedInViewer?.Invoke(viewer, EventArgs.Empty);
			}

			public static bool InvokePrintingDocumentInViewer(StiReport report)
			{
				if (GlobalEvents.PrintingDocumentInViewer != null)
				{
					GlobalEvents.PrintingDocumentInViewer(report, EventArgs.Empty);
					return true;
				}
				return false;
			}

			public static void InvokeStartEditingInPreview(object sender, EventArgs e)
			{
				GlobalEvents.StartEditingInPreview?.Invoke(sender, e);
			}

			public static void InvokeEndEditingInPreview(object sender, EventArgs e)
			{
				GlobalEvents.EndEditingInPreview?.Invoke(sender, e);
			}

			public static void InvokeEnableEditorMode(object sender, EventArgs e)
			{
				GlobalEvents.EnableEditorMode?.Invoke(sender, e);
			}

			public static void InvokeDisableEditorMode(object sender, EventArgs e)
			{
				GlobalEvents.DisableEditorMode?.Invoke(sender, e);
			}

			public static void InvokeClick(object sender, EventArgs e)
			{
				GlobalEvents.Click?.Invoke(sender, e);
			}

			public static void InvokeMouseClick(object sender, MouseEventArgs e)
			{
				MouseEventHandler mouseClick = GlobalEvents.MouseClick;
				if (mouseClick != null)
				{
					mouseClick.Invoke(sender, e);
				}
			}

			public static void InvokeDoubleClick(object sender, EventArgs e)
			{
				GlobalEvents.DoubleClick?.Invoke(sender, e);
			}

			public static bool InvokeHyperlinkClick(object viewer, string hyperlink)
			{
				if (GlobalEvents.HyperlinkClick == null)
				{
					return false;
				}
				StiHyperlinkClickEventArgs stiHyperlinkClickEventArgs = new StiHyperlinkClickEventArgs(hyperlink);
				GlobalEvents.HyperlinkClick(viewer, stiHyperlinkClickEventArgs);
				return stiHyperlinkClickEventArgs.Handled;
			}

			public static void InvokeCheckDrillDownReport(object sender, StiCheckDrillDownReportEventArgs e)
			{
				GlobalEvents.CheckDrillDownReport?.Invoke(sender, e);
			}

			public static void InvokeGetDrillDownReport(object sender, StiGetDrillDownReportEventArgs e)
			{
				GlobalEvents.GetDrillDownReport?.Invoke(sender, e);
			}

			public static void InvokeNewRequestFromUserDialogCreating(StiNewRequestFromUserDialogCreatingEventArgs e)
			{
				GlobalEvents.NewRequestFromUserDialogCreating?.Invoke(null, e);
			}
		}

		public sealed class Globalization
		{
			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowUseText { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowUseTag { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowUseToolTip { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowUseHyperlink { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			public static bool AllowUseVariableAlias { get; set; } = true;

		}

		public static class Image
		{
			[DefaultValue(null)]
			[Description("Gets or sets absolute path which will be used in combination with path from File property of image component.")]
			[StiSerializable]
			public static string AbsolutePathOfImages { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value forcing image cloning.")]
			[StiSerializable]
			[Category("Engine")]
			public static bool UseImageCloning { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value forcing rotate image by Exif orientation data.")]
			[StiSerializable]
			[Category("Engine")]
			public static bool RotateImageByExifOrientationData { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value forcing converting SVG-images to the bitmap for output.")]
			[StiSerializable]
			[Category("Engine")]
			public static bool ConvertSvgToBitmap { get; set; } = true;

		}

		public sealed class ImageCache
		{
			[DefaultValue(null)]
			[Description("Specifies path for the image cache.")]
			[StiSerializable]
			public static string CachePath { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			public static bool Enabled { get; set; }
		}

		public sealed class Interaction
		{
			[DefaultValue(false)]
			[StiSerializable]
			public static bool ComboBoxDropDownAutoWidth { get; set; }

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ForceSortingWithFullTypeConversion { get; set; } = true;

		}

		public sealed class QuickInfo
		{
			[StiSerializable]
			[StiOptionsFontHelper(0)]
			public static Font QuickInfoSystemFont { get; set; } = new Font("Arial", 7f);


			public static StringFormat QuickInfoStringFormat { get; set; } = new StringFormat
			{
				FormatFlags = (StringFormatFlags)4096,
				Alignment = (StringAlignment)2,
				Trimming = (StringTrimming)0
			};


			public static Pen DashStylePen { get; set; } = new Pen(Color.DimGray)
			{
				DashStyle = (DashStyle)1
			};

		}

		public sealed class ReportCache
		{
			private static int amountOfQuickAccessPages = 50;

			private static int limitForStartUsingCache = 200;

			private static int amountOfProcessedPagesForStartGCCollect = 60;

			[DefaultValue(50)]
			[Description("Gets or sets amount of quick access pages. This value must be greater than 5.")]
			[StiSerializable]
			public static int AmountOfQuickAccessPages
			{
				get
				{
					return amountOfQuickAccessPages;
				}
				set
				{
					if (value < 5)
					{
						throw new ArgumentException("Property AmountOfQuickAccessPages can't be less then 5.");
					}
					amountOfQuickAccessPages = value;
				}
			}

			[DefaultValue(200)]
			[Description("Gets or sets lower bound of starting using the cache of the report.")]
			[StiSerializable]
			public static int LimitForStartUsingCache
			{
				get
				{
					return limitForStartUsingCache;
				}
				set
				{
					if (value < 0)
					{
						throw new ArgumentException("Property LimitForStartUsingCache can't be less then 0.");
					}
					limitForStartUsingCache = value;
				}
			}

			[DefaultValue(60)]
			[Description("Gets or sets lower bound of starting the Garbage Collector.")]
			[StiSerializable]
			public static int AmountOfProcessedPagesForStartGCCollect
			{
				get
				{
					return amountOfProcessedPagesForStartGCCollect;
				}
				set
				{
					if (value < 0)
					{
						throw new ArgumentException("Property AmountOfProcessedPagesForStartGCCollect can't be less then 0.");
					}
					amountOfProcessedPagesForStartGCCollect = value;
				}
			}

			[DefaultValue(true)]
			[Description("Gets or sets value which indicates that report engine can start the Garbage Collector during report rendering in the report cache.")]
			[StiSerializable]
			public static bool AllowGCCollect { get; set; } = true;


			[DefaultValue(StiReportCacheThreadMode.Auto)]
			[Description("Gets or sets a thread mode.")]
			[StiSerializable]
			[Category("Engine")]
			public static StiReportCacheThreadMode ThreadMode { get; set; } = StiReportCacheThreadMode.Auto;


			[DefaultValue(null)]
			[Description("Gets or sets value which specifies path for the report cache.")]
			[StiSerializable]
			public static string CachePath { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets value which indicates that report engine can dispose the images during report rendering in the report cache.")]
			[StiSerializable]
			public static bool DisposeImagesOnPageClear { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets value which indicates that optimization is used for SetText on EndRender.")]
			[StiSerializable]
			public static bool OptimizeEndRenderSetText { get; set; } = true;

		}

		public sealed class ReportResources
		{
			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("Property 'AllowUseResourcesInWinForms' is obsolete. Now report engine automatically decide how to store resources.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool AllowUseResourcesInWinForms { get; set; } = true;


			[DefaultValue(true)]
			[StiSerializable]
			[Obsolete("Property 'AllowUseResourcesInWPF' is obsolete. Now report engine automatically decide how to store resources.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool AllowUseResourcesInWPF { get; set; } = true;


			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("Property 'AllowUseResourcesInWeb' is obsolete. Now report engine automatically decide how to store resources.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool AllowUseResourcesInWeb { get; set; }

			[DefaultValue(null)]
			[StiSerializable]
			public static string ResourcesPath { get; set; }

			[DefaultValue(5000000)]
			[StiSerializable]
			public static int MaximumSize { get; set; } = 5000000;

		}

		public sealed class RtfCache
		{
			[DefaultValue(null)]
			[Description("Specifies path for the rtf cache.")]
			[StiSerializable]
			public static string CachePath { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			public static bool Enabled { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			public static bool AllowGCCollect { get; set; }
		}

		public sealed class TextDrawingMeasurement
		{
			[DefaultValue(1.0)]
			[StiSerializable]
			public static double MeasurementFactorStandard { get; set; } = 1.0;


			[DefaultValue(1.0)]
			[StiSerializable]
			public static double MeasurementFactorWysiwyg { get; set; } = 1.0;


			[DefaultValue(1.0)]
			[StiSerializable]
			public static double MeasurementFactorTypographic { get; set; } = 1.0;


			[DefaultValue(1.0)]
			[StiSerializable]
			public static double MeasurementFactorWpf { get; set; } = 1.0;


			[DefaultValue(1.1)]
			[StiSerializable]
			public static double MeasurementFactorWpfWithHtmlInCrossTab { get; set; } = 1.1;


			[DefaultValue(false)]
			[StiSerializable]
			public static bool MeasurementWpfCompatibility2016 { get; set; }
		}

		public sealed class Watermark
		{
			[DefaultValue(false)]
			[StiSerializable]
			public static bool AllowExpression { get; set; } = true;

		}

		public sealed class WYSIWYG
		{
			private static bool interpreteFontSizeInHtmlTagsAsInHtml;

			[DefaultValue(4.0)]
			[StiSerializable]
			[Obsolete("StiOptions.Engine.WYSIWYG.PrecisionModeFactor property is obsolete.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static double PrecisionModeFactor
			{
				get
				{
					return StiTextRenderer.PrecisionModeFactor;
				}
				set
				{
					StiTextRenderer.PrecisionModeFactor = value;
				}
			}

			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("StiOptions.Engine.WYSIWYG.PrecisionModeEnabled property is obsolete.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool PrecisionModeEnabled
			{
				get
				{
					return StiTextRenderer.PrecisionModeEnabled;
				}
				set
				{
					StiTextRenderer.PrecisionModeEnabled = value;
				}
			}

			[DefaultValue(true)]
			[StiSerializable]
			public static bool CorrectionEnabled
			{
				get
				{
					if (StiTextRenderer.CorrectionEnabled)
					{
						return !StiTextRenderer.Compatibility2009;
					}
					return false;
				}
				set
				{
					StiTextRenderer.CorrectionEnabled = value;
				}
			}

			[DefaultValue(false)]
			[Description("Interprete the FontSize attribute in the Html-tags as in Html.")]
			[StiSerializable]
			public static bool InterpreteFontSizeInHtmlTagsAsInHtml
			{
				get
				{
					if (FullTrust)
					{
						return StiTextRenderer.InterpreteFontSizeInHtmlTagsAsInHtml;
					}
					return interpreteFontSizeInHtmlTagsAsInHtml;
				}
				set
				{
					if (FullTrust)
					{
						StiTextRenderer.InterpreteFontSizeInHtmlTagsAsInHtml = value;
					}
					else
					{
						interpreteFontSizeInHtmlTagsAsInHtml = value;
					}
				}
			}
		}

		private static string[] defaultNamespaces = new string[9] { "System", "System.Drawing", "System.Windows.Forms", "System.Data", "Stimulsoft.Controls", "Stimulsoft.Base.Drawing", "Stimulsoft.Report", "Stimulsoft.Report.Dialogs", "Stimulsoft.Report.Components" };

		private static string[] namespaces;

		private static string[] defaultReferencedAssemblies = new string[8] { "System.Dll", "System.Drawing.Dll", "System.Windows.Forms.Dll", "System.Data.Dll", "System.Xml.Dll", "Stimulsoft.Controls.Dll", "Stimulsoft.Base.Dll", "Stimulsoft.Report.Dll" };

		private static string[] referencedAssemblies;

		[DefaultValue(false)]
		[Description("Gets or sets a value which controls of output barcodes as bitmaps in WebViewerFx.")]
		[StiSerializable]
		[Category("BarCode")]
		public static bool RenderBarCodeAsBitmap { get; set; }

		[DefaultValue(3f)]
		[Description("Gets or sets a value of barcode dpi multiplier factor.")]
		[StiSerializable]
		[Category("Engine")]
		public static float BarcodeDpiMultiplierFactor { get; set; } = 3f;


		[DefaultValue(StiQRCodeECIMode.ISO_8859_1)]
		[Description("Gets or sets a value of a default encoding of byte mode.")]
		[StiSerializable]
		[Category("Engine")]
		public static StiQRCodeECIMode BarcodeQRCodeDefaultByteModeEncoding { get; set; } = StiQRCodeECIMode.ISO_8859_1;


		[DefaultValue(true)]
		[Description("Gets or sets a value which allow to use BOM for the Unicode encoding in the QR-Code.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool BarcodeQRCodeAllowUnicodeBOM { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value which allow to use BOM for the Unicode encoding in the QR-Code.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool BarcodeImproveQualityHtmlExport { get; set; } = false;


		public static bool AllowFixOldChartTitle
		{
			get
			{
				return StiSerializing.AllowFixOldChartTitle;
			}
			set
			{
				StiSerializing.AllowFixOldChartTitle = value;
			}
		}

		[DefaultValue(true)]
		[Description("This value indicates that it is necessary to simulate interactivity for charts with text elements.")]
		[StiSerializable]
		[Category("Chart")]
		public static bool AllowFixPieChartMarkerAlignment { get; set; }

		[DefaultValue(false)]
		[Description("This value indicates that it is necessary to simulate interactivity for charts with text elements.")]
		[StiSerializable]
		[Category("Chart")]
		public static bool AllowInteractionInChartWithComponents { get; set; }

		[DefaultValue(true)]
		[Description("The default value for the AllowApplyStyle property of the StiChart component.")]
		[StiSerializable]
		[Category("Chart")]
		public static bool DefaultValueOfAllowApplyStyleProperty { get; set; } = true;


		[DefaultValue(true)]
		[Description("The value allows applying a chart style to self-series.")]
		[StiSerializable]
		[Category("Chart")]
		public static bool ApplyStylesInAutoSeries { get; set; } = true;


		[DefaultValue(false)]
		[Description("Allow using the old mode of formatting percent in charts.")]
		[StiSerializable]
		[Category("Chart")]
		public static bool OldChartPercentMode
		{
			get
			{
				return StiChartOptions.OldChartPercentMode;
			}
			set
			{
				StiChartOptions.OldChartPercentMode = value;
			}
		}

		[DefaultValue(false)]
		[Description("Gets or sets a value which controls of output charts as bitmaps.")]
		[StiSerializable]
		[Category("Chart")]
		public static bool RenderChartAsBitmap { get; set; }

		[DefaultValue(2f)]
		[Description("Gets or sets a scalling factor for rendering charts as bitmaps.")]
		[StiSerializable]
		[Category("Chart")]
		public static float RenderChartAsBitmapZoom { get; set; } = 2f;


		[DefaultValue(false)]
		[Description("A value indicates that the InvokeProcessChart event will be invoked for the template chart component.")]
		[StiSerializable]
		[Category("Chart")]
		public static bool AllowInvokeProcessChartEventForTemplateOfChart { get; set; }

		[DefaultValue(false)]
		[Description("A value indicates that the chart values will be simplified.")]
		[StiSerializable]
		[Category("Chart")]
		public static bool AllowSimplifyChartValues { get; set; }

		[DefaultValue(false)]
		[Description("If the property is set to true, then, before chart rendering, the report engine will not remember a position in the data source used for the chart.")]
		[StiSerializable]
		[Category("Chart")]
		public static bool DontSaveDataSourceBeforeChartRendering { get; set; }

		[DefaultValue(false)]
		[Description("A value indicates that the text in chart can contain an html-tags.")]
		[StiSerializable]
		[Category("Chart")]
		public static bool AllowHtmlTagsInChart { get; set; }

		[DefaultValue(true)]
		[Description("If true, the report engine can forcibly set the CanBreak property to true for DataBand, if the CrossTab component with the enabled PrintTitlesOnAllPages property is put on this DataBand.")]
		[StiSerializable]
		[Category("CrossTab")]
		[Obsolete("This property is not work anymore.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool AllowForceCanBreakForCrossTabPrintOnAllPages { get; set; } = true;


		[StiOptionsUniversalTypeObject(StiOptionsUniversalType.Enum, "Stimulsoft.Report.StiReportUnitType, Stimulsoft.Report")]
		[Description("This property stores units? values by default. If null, then millimeters of or hundredths of inches are used, depending on the regional settings.")]
		[StiSerializable]
		[Category("Engine")]
		[DefaultValue(null)]
		public static object DefaultUnit { get; set; }

		[DefaultValue(true)]
		[Description("This value indicates that the OriginalFontName property should be used when serializing a report. The OriginalFontName property stores the name of the original font name, even if this font is not present in the system. Using this option is not recommended in ASP.NET.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowUseOriginalFontName { get; set; } = true;


		[DefaultValue(true)]
		[Description("Allows to reset properties (TagValue, HyperlinkValue, BookmarkValue and ToolTipValue) to null before report rendering.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowResetValuesAtComponent { get; set; } = true;


		[DefaultValue(false)]
		[Description("The default value for the PrintIfDetailEmpty property of the DataBand component.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool PrintIfDetailEmptyDefaultValue { get; set; }

		[DefaultValue(true)]
		[Description("If the property is set to true, then the library may change the CurrentDurectory property.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowSetCurrentDirectory
		{
			get
			{
				return StiBaseOptions.AllowSetCurrentDirectory;
			}
			set
			{
				StiBaseOptions.AllowSetCurrentDirectory = value;
			}
		}

		[DefaultValue(true)]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowResetAssemblyAfterCompilation { get; set; } = true;


		[DefaultValue(false)]
		[Description("If the value is set to true, then the progress window of report rendering is run in another thread.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowProgressInThread { get; set; }

		[DefaultValue(false)]
		[Description("If the value is set to true, then the Progress property of the StiReport object can be used for the progress customization.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowProgressExternalAccess { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets value which indicates that config file 'Stimulsoft.Report.config' is not saved from report engine.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool DontSaveConfig { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets value which indicates that config file 'Stimulsoft.Report.settings' is not saved from report engine.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool DontSaveSettings { get; set; }

		[DefaultValue(/*Could not decode attribute arguments.*/)]
		[Description("Gets or sets the default report engine for new report.")]
		[StiSerializable]
		[Category("Engine")]
		public static StiEngineVersion DefaultEngineVersion { get; set; } = (StiEngineVersion)2;


		[Obsolete("UseAdvancedPrintOn property is obsolete. Please use UseAdvancedPrintOnEngineV1 property instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool UseAdvancedPrintOn
		{
			get
			{
				return UseAdvancedPrintOnEngineV1;
			}
			set
			{
				UseAdvancedPrintOnEngineV1 = value;
			}
		}

		[DefaultValue(true)]
		[Description("If the property is set to true, the report generator will use the PrintOn property when rendering a report. Using this mode is not recommended because of problems with page numbers. If the property is set to false, then the property will be processed after rendering a report.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool UseAdvancedPrintOnEngineV1 { get; set; } = true;


		[DefaultValue(false)]
		[Description("If the property is set to true, the report generator will use the PrintOn property when rendering a report. Using this mode is not recommended because of problems with page numbers. If the property is set to false, then the property will be processed after rendering a report.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool UseAdvancedPrintOnEngineV2 { get; set; }

		[DefaultValue(false)]
		[Description("If the value is set to true, the names of the new components are generated as localized regardless the settings of the report designer.")]
		[StiSerializable]
		[Category("Naming")]
		public static bool ForceGenerationLocalizedName { get; set; }

		[DefaultValue(false)]
		[Description("If the value is set to true, the names of the new components are generated as not localized regardless the settings of the report designer.")]
		[StiSerializable]
		[Category("Naming")]
		public static bool ForceGenerationNonLocalizedName { get; set; }

		[DefaultValue(true)]
		[Description("If the property is set to true, the report generator may use messages of report rendering.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool ShowReportRenderingMessages { get; set; } = true;


		[DefaultValue(false)]
		[Description("If the property is set to true, the report generator creates a report class with the modifier partial.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool GeneratePartialReportClass { get; set; }

		[DefaultValue(null)]
		[Description("Gets or sets encoding which used for saving soure code.")]
		[StiSerializable]
		[Category("Designer")]
		public static Encoding SourceCodeEncoding { get; set; }

		[DefaultValue(null)]
		[Description("Gets or sets path to AssemblyKeyFile for signing all compiled reports.")]
		[StiSerializable]
		[Category("Designer")]
		public static string PathToAssemblyKeyFile { get; set; }

		[DefaultValue(false)]
		[Description("A value indicates that the ParentReport property of a report is not cleared after rendering process.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool NotClearParentReport { get; set; }

		[DefaultValue(true)]
		[Description("A value indicates which side of a page must use the report designer to dock a page footer component. If the value is true, the report designer will use bottom side of the page.")]
		[StiSerializable]
		[Category("Designer")]
		public static bool DockPageFooterToBottom { get; set; } = true;


		[DefaultValue("Reports")]
		[Description("Gets or sets default namespace for the report.")]
		[StiSerializable]
		[Category("Engine")]
		public static string DefaultNamespace { get; set; } = "Reports";


		[Category("Engine")]
		public static string[] Namespaces
		{
			get
			{
				return namespaces ?? defaultNamespaces;
			}
			set
			{
				namespaces = value;
			}
		}

		public static bool IsDefaultNamespaces => namespaces == null;

		[Category("Engine")]
		public static string[] ReferencedAssemblies
		{
			get
			{
				return referencedAssemblies ?? defaultReferencedAssemblies;
			}
			set
			{
				referencedAssemblies = value;
			}
		}

		public static bool IsDefaultReferencedAssemblies => referencedAssemblies == null;

		[DefaultValue(StiNamingRule.Advanced)]
		[Description("Gets or sets a value which controls of naming of new components in the report.")]
		[StiSerializable]
		[Category("Naming")]
		public static StiNamingRule NamingRule { get; set; } = StiNamingRule.Advanced;


		[DefaultValue(StiReportLanguageType.CSharp)]
		[Description("Gets or sets the default programming language of the report.")]
		[StiSerializable]
		[Category("Engine")]
		public static StiReportLanguageType DefaultReportLanguage { get; set; } = StiReportLanguageType.CSharp;


		[Category("Engine")]
		public static Type BaseReportType { get; set; } = typeof(StiReport);


		[DefaultValue(true)]
		[StiSerializable]
		[Category("Engine")]
		public static bool FullTrust
		{
			get
			{
				return StiBaseOptions.FullTrust;
			}
			set
			{
				StiBaseOptions.FullTrust = value;
			}
		}

		[DefaultValue(false)]
		[StiSerializable]
		[Category("Engine")]
		public static bool FIPSCompliance
		{
			get
			{
				return StiBaseOptions.FIPSCompliance;
			}
			set
			{
				StiBaseOptions.FIPSCompliance = value;
			}
		}

		[DefaultValue(false)]
		[StiSerializable]
		[Category("Engine")]
		public static bool HideRenderingProgress { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets a value indicating whether not to show the message from engine of the message.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool HideMessages
		{
			get
			{
				return StiExceptionProvider.HideMessages;
			}
			set
			{
				StiExceptionProvider.HideMessages = value;
			}
		}

		[DefaultValue(false)]
		[Description("Gets or sets the value, which indicates not to show the exception from engine of the exception.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool HideExceptions
		{
			get
			{
				return StiExceptionProvider.HideExceptions;
			}
			set
			{
				StiExceptionProvider.HideExceptions = value;
			}
		}

		[DefaultValue(false)]
		[Description("Gets or sets a value which enables or disables log of the report.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool LogEnabled
		{
			get
			{
				return StiLogService.LogEnabled;
			}
			set
			{
				StiLogService.LogEnabled = value;
			}
		}

		[DefaultValue(false)]
		[Description("Gets or sets a value, which indicates whether it is necessary to show tracing messages of the report engine in the Visual Studio Message Window.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool TraceEnabled
		{
			get
			{
				return StiLogService.TraceEnabled;
			}
			set
			{
				StiLogService.TraceEnabled = value;
			}
		}

		[DefaultValue(/*Could not decode attribute arguments.*/)]
		[Description("Gets or sets default paper size for new created pages.")]
		[StiSerializable]
		[Category("Engine")]
		public static PaperKind DefaultPaperSize { get; set; } = (PaperKind)0;


		[DefaultValue(false)]
		[StiSerializable]
		[Category("Engine")]
		public static bool IgnoreFirstPassForGroup { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		[Category("Engine")]
		public static bool OldModeOfRenderingEventInEngineV2 { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		[Category("Engine")]
		public static bool UseGarbageCollectorForImageCache { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		[Category("Export")]
		public static bool ConvertBmpDataToPng { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		[Category("Engine")]
		public static bool UsePercentageProgress { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		[Category("Engine")]
		public static bool UseTemplateForPagePrintEvents { get; set; } = true;


		[DefaultValue(false)]
		[StiSerializable]
		[Category("Engine")]
		public static bool UseParentStylesOldMode { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		[Category("Engine")]
		public static bool ThrowErrorIfFontIsNotInstalledInSystem { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		[Category("Engine")]
		public static bool UseCollateOldMode { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		[Category("Engine")]
		public static bool ForceNewPageInSubReports { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		[Category("Engine")]
		public static bool DpiAware { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		[Category("Data")]
		public static bool AllowUseResetMethodInBusinessObject { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value which indicates whether use the back compatibility when Style property of primitives is set to None.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool PrimitivesStyleNoneBackCompatibility { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value which indicates whether use the CheckSize method for Continued containers.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool UseCheckSizeForContinuedContainers { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value which indicates whether check DockToContainer if component is disabled.")]
		[StiSerializable]
		[Category("Designer")]
		public static bool CheckDockToContainerIfComponentDisabled { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value which indicates whether force the NewPage method for the extra columns.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool ForceNewPageForExtraColumns { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value which indicates whether use Round() for the ToCurrencyWords functions.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool UseRoundForToCurrencyWordsFunctions { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value which indicates whether force the normalization of EndOfLine symbols.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool ForceNormalizeEndOfLineSymbols { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value which indicates whether force to disable the collapsing.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool ForceDisableCollapsing { get; set; }

		[DefaultValue(true)]
		[Description("This value indicates that it is necessary to pack script after report have been loaded.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool PackScriptAfterReportLoaded { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value which allow to use the PrintOnAllPages property of headers in subreports.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool UsePrintOnAllPagesPropertyOfHeadersInSubreports { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value indicating whether it is necessary to force the TopMost property of the ProgressForm.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool ForceTopMostPropertyOfProgressForm { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets a value indicating whether it is necessary to fix state of first collapsing band. For compatibility with 2012.1 set this property to true.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool FixFirstCollapsingBandState { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value indicating whether it is necessary to render external subreports with help of unlimited height pages.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool RenderExternalSubReportsWithHelpOfUnlimitedHeightPages { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value forcing optimization in LoadDocument method.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool DocumentLoadingOptimization { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets a value forcing optimization in SaveDocument method.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool DocumentSavingOptimization { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value forcing optimization in serialization methods.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool SerializationOptimization { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value forcing removing comments from events script.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool RemoveCommentsFromEventScript { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value forcing fix conversion of argument for SumTime function.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowFixSumTimeArgumentConversion { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value forcing fix conversion of DateTime argument for functions.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowFixDateTimeArgumentConversion { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value forcing aggregate functions exception processing.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool ForceAggregateFunctionsExceptionProcessing { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value forcing interpretation mode of the report calculation.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool ForceInterpretationMode { get; set; } = false;


		[DefaultValue(false)]
		[Description("Gets or sets a value which defines a previous realization of the Paint mode of the ZipCode component.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool UseOldZipCodePaintMode { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets a value forcing removing bottom border for breaked container.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool RemoveBottomBorderOfSplitContainer { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets a value forcing stop processing conditions at first true condition.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool StopProcessingAtFirstTrueCondition { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets a value forcing invert the order of the conditions processing. For compatibility with 2015.2 set this property to true.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool InvertConditionsProcessingOrder { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets a value forcing apply condition style for disabled components.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool ApplyConditionStyleForDisabledComponents { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value which allow BreakContainer optimization.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowBreakContainerOptimization { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value which allow use generic types in CodeDom serialization.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowUseGenericTypesInCodeDom { get; set; }

		[DefaultValue(MidpointRounding.ToEven)]
		[Description("Gets or sets a value which defines a midpoint rounding mode.")]
		[StiSerializable]
		[Category("Engine")]
		public static MidpointRounding MidpointRounding { get; set; } = MidpointRounding.ToEven;


		[DefaultValue(false)]
		[Description("Gets or sets a value which allow extended check for PrintIfDetailEmpty property.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool PrintIfDetailEmptyNesting { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value indicating whether it is necessary to fix the PageNumber value in the rendering events. For compatibility with 2016.3 set this property to false.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool FixPageNumberInEvents { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value which allow insert line breaks when saving byte array to Base64 string.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowInsertLineBreaksWhenSavingByteArray
		{
			get
			{
				return StiBaseOptions.AllowInsertLineBreaksWhenSavingByteArray;
			}
			set
			{
				StiBaseOptions.AllowInsertLineBreaksWhenSavingByteArray = value;
			}
		}

		[DefaultValue(true)]
		[Description("Gets or sets a value indicating whether it is possible to return zero result for Min/Max aggregate functions if there is no acceptable input data. For compatibility with 2017.2 set this property to false.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowZeroResultForMinMaxAggregateFunctions { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value which allows to load Stimulsoft Dashboards assembly.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool SkipLoadingDashboardAssembly { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets a value which allows to load Stimulsoft Apps assembly.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool SkipLoadingAppAssembly { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value which allows converting values to required types in the text formatting.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowConvertingInFormatting { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value which allows old style formatting of a time span value.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowOldTimeSpanFormatting { get; set; }

		[Description("Gets or sets a color which is used for coloring negative values in the text formatting.")]
		[StiSerializable]
		[Category("Engine")]
		public static Color NegativeColor { get; set; } = Color.Red;


		[DefaultValue(false)]
		[Description("Gets or sets a value which force disable Antialiasing in painters.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool DisableAntialiasingInPainters { get; set; } = false;


		[DefaultValue(false)]
		[Description("Gets or sets a value which force using gap for last column of container.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowGapForLastColumn { get; set; } = false;


		[DefaultValue(false)]
		[Description("Gets or sets a value which force sort the list using the ordinal value of the character code points (StringComparer.Ordinal).")]
		[StiSerializable]
		[Category("Engine")]
		public static bool OrdinalStringComparison { get; set; } = false;


		[DefaultValue(false)]
		[Description("Gets or sets a value which force show Bookmark for disabled band.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool ShowBookmarkToDisabledBand { get; set; } = false;


		[DefaultValue(StiParserType.DataParser)]
		[Description("Specifies a type of script parser for the text element expression.")]
		[StiSerializable]
		[Category("Dashboard")]
		public static StiParserType DashboardTextElementExpressionParser { get; set; } = StiParserType.DataParser;


		[DefaultValue(false)]
		[Description("If true then database information will be required from database with help of DataAdapter (instead Connection.GetSchema).")]
		[StiSerializable]
		[Category("Data")]
		public static bool AdvancedRetrievalModeOfDatabaseInformation
		{
			get
			{
				return StiDataConnector.AdvancedRetrievalModeOfDatabaseSchema;
			}
			set
			{
				StiDataConnector.AdvancedRetrievalModeOfDatabaseSchema = value;
			}
		}

		[DefaultValue(true)]
		[Description("If true then in GetSchema method, column reference by index else by name.")]
		[StiSerializable]
		[Category("Data")]
		public static bool GetSchemaColumnsMode
		{
			get
			{
				return StiDataConnector.GetSchemaColumnsMode;
			}
			set
			{
				StiDataConnector.GetSchemaColumnsMode = value;
			}
		}

		[DefaultValue(/*Could not decode attribute arguments.*/)]
		[Description("StiOptions.Engine.RetriveColumnsMode was obsoleted. Please use StiOptions.Engine.RetrieveColumnsMode.")]
		[StiSerializable]
		[Category("Data")]
		[Obsolete("StiOptions.Engine.RetriveColumnsMode was obsoleted. Please use StiOptions.Engine.RetrieveColumnsMode.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[StiBrowsable(false)]
		public static StiRetrieveColumnsMode RetriveColumnsMode
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				return RetrieveColumnsMode;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				RetrieveColumnsMode = value;
			}
		}

		[DefaultValue(/*Could not decode attribute arguments.*/)]
		[Description("Value which describes mode of retrieving database information.")]
		[StiSerializable]
		[Category("Data")]
		public static StiRetrieveColumnsMode RetrieveColumnsMode
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				return StiDataOptions.RetrieveColumnsMode;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				StiDataOptions.RetrieveColumnsMode = value;
			}
		}

		[DefaultValue(/*Could not decode attribute arguments.*/)]
		[Description("StiOptions.Engine.WizardStoredProcRetriveMode was obsoleted. Please use StiOptions.Engine.WizardStoredProcRetrieveMode.")]
		[StiSerializable]
		[Category("Data")]
		[StiBrowsable(false)]
		[Browsable(false)]
		[Obsolete("StiOptions.Engine.WizardStoredProcRetriveMode was obsoleted. Please use StiOptions.Engine.WizardStoredProcRetrieveMode.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static StiWizardStoredProcRetriveMode WizardStoredProcRetriveMode
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				return (StiWizardStoredProcRetriveMode)WizardStoredProcRetrieveMode;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				WizardStoredProcRetrieveMode = (StiWizardStoredProcRetrieveMode)value;
			}
		}

		[DefaultValue(/*Could not decode attribute arguments.*/)]
		[Description("Value which describes mode of retrieving information for stored procedures.")]
		[StiSerializable]
		[Category("Data")]
		public static StiWizardStoredProcRetrieveMode WizardStoredProcRetrieveMode
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				return StiDataOptions.WizardStoredProcRetrieveMode;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				StiDataOptions.WizardStoredProcRetrieveMode = value;
			}
		}

		[DefaultValue(true)]
		[Description("If true then will be call Prepare() method for queries for each run.")]
		[StiSerializable]
		[Category("Data")]
		public static bool AllowPrepareSqlQueries { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value, which indicates that if data are absent then controls of the data emulation. This value is used for report rendering in the designer without data.")]
		[StiSerializable]
		[Category("Designer")]
		public static bool EmulateData { get; set; }

		[DefaultValue(false)]
		[Description("If true then exception will be generated if column does not exists.")]
		[StiSerializable]
		[Category("Data")]
		public static bool AllowThrowExceptionWhenColumnDoesNotExists { get; set; }

		[DefaultValue(true)]
		[Description("If true then filter data in DataSource before sorting. For backward compatibility with 2016.1.")]
		[StiSerializable]
		[Category("Data")]
		public static bool FilterDataInDataSourceBeforeSorting { get; set; } = true;


		[DefaultValue(false)]
		[Description("Reconnect DataSources used in the RequestFromUser variables if value of the variable was changed.")]
		[StiSerializable]
		[Category("Data")]
		public static bool ReconnectDataSourcesIfRequestFromUserVariableChanged { get; set; }

		[DefaultValue(true)]
		[Description("If true then remove double call of SetData method for detail bands. For backward compatibility with 2016.1.")]
		[StiSerializable]
		[Category("Data")]
		public static bool OptimizeDetailDataFiltering { get; set; } = true;


		[DefaultValue(true)]
		[Description("Set schema name for retrieve information for PostgreSql")]
		[StiSerializable]
		[Category("Data")]
		public static string RetrieveSchemaNamePostgreSql
		{
			get
			{
				return StiBaseOptions.RetrieveSchemaNamePostgreSql;
			}
			set
			{
				StiBaseOptions.RetrieveSchemaNamePostgreSql = value;
			}
		}

		[DefaultValue(false)]
		[Description("If true then call the DisconnectFromData method before the EndRender event. For backward compatibility with 2016.3.")]
		[StiSerializable]
		[Category("Data")]
		public static bool DisconnectFromDataBeforeEndRender { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets a value indicating whether it is necessary to insert breaks in Rtf export.")]
		[StiSerializable]
		[Category("Export")]
		public static bool ForceRtfBreak { get; set; }

		[DefaultValue(null)]
		[StiSerializable]
		[Category("Export")]
		public static Font DefaultRtfFont { get; set; }

		[DefaultValue(/*Could not decode attribute arguments.*/)]
		[StiSerializable]
		[Category("Engine")]
		public static EmfType RichTextDrawingMetafileType { get; set; } = (EmfType)3;


		[DefaultValue(500)]
		[StiSerializable]
		[Category("Engine")]
		public static int RichTextImageCacheSize { get; set; } = 500;


		[DefaultValue(false)]
		[Description("Gets or sets a value forcing render RichText components in other application domain.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool RenderRichTextInOtherDomain { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value forcing load extended RichText library.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool? ForceLoadExtendedRichTextLibrary
		{
			get
			{
				return StiBaseOptions.ForceLoadExtendedRichTextLibrary;
			}
			set
			{
				StiBaseOptions.ForceLoadExtendedRichTextLibrary = value;
			}
		}

		[DefaultValue("RichEdit50W")]
		[Description("Gets or sets a value which specified the extended RichText library ClassName.")]
		[StiSerializable]
		[Category("Engine")]
		public static string ExtendedRichTextLibraryClassName
		{
			get
			{
				return StiBaseOptions.ExtendedRichTextLibraryClassName;
			}
			set
			{
				StiBaseOptions.ExtendedRichTextLibraryClassName = value;
				StiRichTextBox stiRichTextBox = new StiRichTextBox(drawTransparent: false);
				((Component)(object)stiRichTextBox).Dispose();
			}
		}

		[DefaultValue(96)]
		[Description("Gets or sets a value of the RichText scaling, in percent.")]
		[StiSerializable]
		[Category("Engine")]
		public static float RichTextScale { get; set; } = 96f;


		[DefaultValue(true)]
		[Description("Gets or sets a value forcing render RichText in Wpf always in Wysiwyg mode.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool RenderRichTextInWpfAlwaysWysiwyg { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value indicating whether it is necessary to check InstalledPrinters for get MeasureGraphics with maximum dpi.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool CheckInstalledPrintersForRichTextMeasureGraphics { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value indicating whether it is necessary to lock RichText processing.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool ForceLockRichTextThread { get; set; } = true;


		[DefaultValue(StiTextQuality.Standard)]
		[Description("The default value for the TextQuality property.")]
		[StiSerializable]
		[Category("Engine")]
		public static StiTextQuality DefaultTextQualityMode { get; set; } = StiTextQuality.Standard;


		[DefaultValue(false)]
		[Description("Gets or sets a value which indicates whether Gdi32 is used for text rendering. For .Net 2.0 only.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool UseGdi32ForTextRendering { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		[Description("A value indicates that text drawing engine will measure text string including trailing spaces.")]
		[Category("Engine")]
		public static bool MeasureTrailingSpaces
		{
			get
			{
				return StiTextDrawing.MeasureTrailingSpaces;
			}
			set
			{
				StiTextDrawing.MeasureTrailingSpaces = value;
			}
		}

		[DefaultValue(false)]
		[StiSerializable]
		[Category("Engine")]
		public static bool UseOldWYSIWYGTextQuality { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets a value which defines a previous realization of the WYSIWYG mode of the StiText component.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool OldWYSIWYG { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value forcing drawing rectangles instead of font with tiny size.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool TinyTextOptimization { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value which allow using cache for GetActualSize methods.")]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowCacheForGetActualSize { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value which allow assign empty string in condition of textbox. For back compatibility with 2016.2")]
		[StiSerializable]
		[Category("Engine")]
		public static bool AllowAssignEmptyStringInCondition { get; set; }

		[DefaultValue(96)]
		[Description("Gets or sets a value of the font scaling for print with Standard TextQuality mode, in percent.")]
		[StiSerializable]
		[Category("Engine")]
		public static float StandardTextQualityScale { get; set; } = 96f;


		[DefaultValue(96)]
		[Description("Gets or sets a value of the font scaling for print with Typographic TextQuality mode, in percent.")]
		[StiSerializable]
		[Category("Engine")]
		public static float TypographicTextQualityScale { get; set; } = 96f;


		[DefaultValue(1f)]
		[Description("Gets or sets a value of the text LineSpacing scaling, usually used for libgdiplus.")]
		[StiSerializable]
		[Category("Engine")]
		public static float TextLineSpacingScale { get; set; } = 1f;


		public static bool MarkBreakedText { get; set; }

		public static bool UseNewHtmlEngine { get; set; } = false;

	}

	public sealed class ExceptionProvider
	{
		[DefaultValue(false)]
		[StiSerializable]
		public static bool DisableSendError
		{
			get
			{
				return StiExceptionProvider.DisableSendError;
			}
			set
			{
				StiExceptionProvider.DisableSendError = value;
			}
		}

		[DefaultValue(null)]
		[StiSerializable]
		public static string ServerUrl
		{
			get
			{
				return StiExceptionProvider.ServerUrl;
			}
			set
			{
				StiExceptionProvider.ServerUrl = value;
			}
		}
	}

	public sealed class Export
	{
		public class CheckBoxReplacementForExcelValue
		{
			[Description("Gets or sets a default font of text.")]
			[StiSerializable]
			[StiOptionsFontHelper(1)]
			public static Font Font { get; set; } = new Font("Arial", 10f);


			[DefaultValue(/*Could not decode attribute arguments.*/)]
			[Description("Gets or sets a default horizontal alignment of text.")]
			[StiSerializable]
			public static StiTextHorAlignment HorAlignment { get; set; } = (StiTextHorAlignment)1;


			[DefaultValue(/*Could not decode attribute arguments.*/)]
			[Description("Gets or sets a default vertical alignment of text.")]
			[StiSerializable]
			public static StiVertAlignment VertAlignment { get; set; } = (StiVertAlignment)1;

		}

		[Obsolete("The class StiOptions.StiCheckBoxExcelValueParameters is obsolete! Please use class StiOptions.CheckBoxReplacementForExcelValue instead it.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class StiCheckBoxExcelValueParameters : CheckBoxReplacementForExcelValue
		{
		}

		public sealed class Csv
		{
			[DefaultValue("")]
			[Description("Gets or sets forced separator for the Csv export.")]
			[StiSerializable]
			public static string ForcedSeparator { get; set; } = string.Empty;


			[DefaultValue(false)]
			[Description("Gets or sets a value which indicates whether it is necessary to use aliases instead of names of components for columns name.")]
			[StiSerializable]
			public static bool UseAliases { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets a value which indicates whether it is necessary to use multiline text.")]
			[StiSerializable]
			public static bool UseMultilineText { get; set; }
		}

		public sealed class Dbf
		{
			[DefaultValue(false)]
			[Description("Gets or sets a value which indicates whether it is necessary to use aliases instead of names of components for columns name.")]
			[StiSerializable]
			public static bool UseAliases { get; set; }
		}

		public class Excel
		{
			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether it is necessary to use image comparer.")]
			[StiSerializable]
			public static bool AllowExportDateTime { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether it is necessary to set columns right to left.")]
			[StiSerializable]
			public static bool ColumnsRightToLeft { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to set flag 'Show Grid Lines'.")]
			[StiSerializable]
			public static bool ShowGridLines { get; set; } = true;


			[DefaultValue(65534)]
			[Description("Gets or sets a value indicating maximum sheet height in rows.")]
			[StiSerializable]
			public static int MaximumSheetHeight { get; set; } = 65534;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to remove empty space at the bottom of page.")]
			[StiSerializable]
			public static bool RemoveEmptySpaceAtBottom { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating behavior of the exporting big cells will be divided into small cells.")]
			[StiSerializable]
			public static bool DivideBigCells { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether it is necessary to use image resolution.")]
			[StiSerializable]
			public static bool UseImageResolution { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to trim trailing spaces.")]
			[StiSerializable]
			public static bool TrimTrailingSpaces { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether it is necessary to allow export Footers and GropuFooters in DataOnly mode.")]
			[StiSerializable]
			[Obsolete("Please use the 'StiExcelExportSettings.DataExportMode' property.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool AllowExportFootersInDataOnlyMode { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use image comparer.")]
			[StiSerializable]
			public static bool AllowImageComparer { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether it is necessary to use the Freeze Panes feature of Excel.")]
			[StiSerializable]
			public static bool AllowFreezePanes { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets a value indicating a Html tags rendering mode.")]
			[StiSerializable]
			public static bool RenderHtmlTagsAsImage { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets a value indicating a checkbox rendering mode.")]
			[StiSerializable]
			public static bool RenderCheckBoxAsImage { get; set; }

			[DefaultValue(StiExcel2007RestrictEditing.No)]
			[StiSerializable]
			public static StiExcel2007RestrictEditing RestrictEditing { get; set; } = StiExcel2007RestrictEditing.No;


			[Description("Gets or sets image format for exported images. Only for Excel2007+.")]
			[StiSerializable]
			public static ImageFormat ImageFormat { get; set; } = null;


			[DefaultValue(1)]
			[Description("Shrink the width of printout to fit a certain number of pages. 0 - Auto.")]
			[StiSerializable]
			public static int NumberOfPagesInWideToFit { get; set; } = 1;


			[DefaultValue(0)]
			[Description("Shrink the height of printout to fit a certain number of pages. 0 - Auto.")]
			[StiSerializable]
			public static int NumberOfPagesInHeightToFit { get; set; }

			[Obsolete("The property FitToOnePageWide is obsolete! Please use NumberOfPagesInWideToFit instead it.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool FitToOnePageWide
			{
				get
				{
					return NumberOfPagesInWideToFit == 1;
				}
				set
				{
					NumberOfPagesInWideToFit = 1;
				}
			}

			[DefaultValue(StiExcelSheetViewMode.Normal)]
			[Description("Gets or sets a value of Excel sheet view mode.")]
			[StiSerializable]
			public static StiExcelSheetViewMode SheetViewMode { get; set; } = StiExcelSheetViewMode.Normal;


			[DefaultValue(false)]
			[Description("Gets or sets a value forcing to move and size images with cells.")]
			[StiSerializable]
			public static bool ImageMoveAndSizeWithCells { get; set; }
		}

		[Obsolete("The class StiOptions.Excel2007 is obsolete! Please use class StiOptions.Excel instead it.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class Excel2007 : Excel
		{
		}

		[Obsolete("The class StiOptions.ExcelXml is obsolete! Please use class StiOptions.Excel instead it.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class ExcelXml : Excel
		{
		}

		public sealed class Html
		{
			[DefaultValue(false)]
			[Description("Gets or sets a value, which indicates whether it is necessary to convert all digits to arabic digits.")]
			[StiSerializable]
			public static bool ConvertDigitsToArabic { get; set; }

			[DefaultValue(StiArabicDigitsType.Standard)]
			[Description("Gets or sets a value indicating a current arabic type of digits.")]
			[StiSerializable]
			public static StiArabicDigitsType ArabicDigitsType { get; set; } = StiArabicDigitsType.Standard;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use image comparer.")]
			[StiSerializable]
			public static bool AllowImageComparer { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether it is necessary to force wordwrap in wysiwyg mode.")]
			[StiSerializable]
			public static bool ForceWysiwygWordwrap { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to replace special characters such as '<', '>', '&'.")]
			[StiSerializable]
			public static bool ReplaceSpecialCharacters { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether it is necessary to use image resolution.")]
			[StiSerializable]
			public static bool UseImageResolution { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use BreakWord mode of wordwrap. For compatibility with 2009.2 and earlier versions set this property to false.")]
			[StiSerializable]
			public static bool UseWordWrapBreakWordMode { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether it is necessary to use strict table cell size. For compatibility with 2012.3 and earlier versions set this property to false.")]
			[StiSerializable]
			public static bool UseStrictTableCellSize { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use strict table cell size. For compatibility with 2016.3 and earlier versions set this property to false.")]
			[StiSerializable]
			public static bool UseStrictTableCellSizeV2 { get; set; } = true;


			public static CultureInfo ForcedCultureForCharts { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether it is necessary to use compatibility with IE6.")]
			[StiSerializable]
			public static bool ForceIE6Compatibility { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use stripped images. For compatibility with 2011.3 and earlier versions set this property to false.")]
			[StiSerializable]
			public static bool AllowStrippedImages { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to remove empty space at the bottom of page.")]
			[StiSerializable]
			public static bool RemoveEmptySpaceAtBottom { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether to use extended style description for fix the styles inheritance problem.")]
			[StiSerializable]
			public static bool UseExtendedStyle { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether to use layout optimization for correct printing.")]
			[StiSerializable]
			public static bool PrintLayoutOptimization { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether to force export chart as bitmap.")]
			[StiSerializable]
			public static bool ChartAsBitmap { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether to use ComponentStyle as the style name.")]
			[StiSerializable]
			public static bool UseComponentStyleName { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to preserve the white spaces. HTML5 export mode is not supported.")]
			[StiSerializable]
			public static bool PreserveWhiteSpaces { get; set; } = true;


			[DefaultValue(null)]
			[Description("Gets or sets a target to open links from the exported report.")]
			[StiSerializable]
			public static string OpenLinksTarget { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to process RoundedRectangles.")]
			[StiSerializable]
			public static bool AllowRoundedRectangles { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to export component from page margins (div/span mode only). For compatibility with 2021.3 and earlier versions set this property to false.")]
			[StiSerializable]
			public static bool ExportComponentsFromPageMargins { get; set; } = true;


			[DefaultValue(3)]
			[Description("Gets or sets a value indicating default coordinates precision.")]
			[StiSerializable]
			public static int DefaultCoordinatesPrecision { get; set; } = 3;

		}

		public class OpenDocumentCalc
		{
			[DefaultValue(true)]
			[Description("Gets or sets a value indicating behavior of the exporting segmented pages will be forcibly broken in order of chosen page format.")]
			[StiSerializable]
			public static bool DivideSegmentPages { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use image comparer.")]
			[StiSerializable]
			public static bool AllowImageComparer { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to remove empty space at the bottom of page.")]
			[StiSerializable]
			public static bool RemoveEmptySpaceAtBottom { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating behavior of the exporting big cells will be divided into small cells.")]
			[StiSerializable]
			public static bool DivideBigCells { get; set; } = true;


			[DefaultValue(1048574)]
			[Description("Gets or sets a value indicating maximum sheet height in rows.")]
			[StiSerializable]
			public static int MaximumSheetHeight { get; set; } = 1048574;

		}

		[Obsolete("The class StiOptions.Ods is obsolete! Please use class StiOptions.OpenDocumentCalc instead it.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class Ods : OpenDocumentCalc
		{
		}

		public class OpenDocumentWriter
		{
			[DefaultValue(true)]
			[Description("Gets or sets a value indicating behavior of the exporting segmented pages will be forcibly broken in order of chosen page format.")]
			[StiSerializable]
			public static bool DivideSegmentPages { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use image comparer.")]
			[StiSerializable]
			public static bool AllowImageComparer { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to remove empty space at the bottom of page.")]
			[StiSerializable]
			public static bool RemoveEmptySpaceAtBottom { get; set; } = true;

		}

		[Obsolete("The class StiOptions.Odt is obsolete! Please use class StiOptions.OpenDocumentWriter instead it.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class Odt : OpenDocumentWriter
		{
		}

		public sealed class Pdf
		{
			public sealed class Security
			{
				[DefaultValue(null)]
				[Description("Gets or sets a default UserPassword.")]
				[StiSerializable]
				public static string DefaultUserPassword { get; set; }

				[DefaultValue(null)]
				[Description("Gets or sets a default OwnerPassword.")]
				[StiSerializable]
				public static string DefaultOwnerPassword { get; set; }
			}

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating behavior of the exporting segmented pages will be forcibly broken in order of chosen page format.")]
			[StiSerializable]
			public static bool DivideSegmentPages { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value which indicates whether it is necessary to convert all digits to arabic digits.")]
			[StiSerializable]
			public static bool ConvertDigitsToArabic { get; set; }

			[DefaultValue(StiArabicDigitsType.Standard)]
			[Description("Gets or sets a value indicating a current arabic type of digits.")]
			[StiSerializable]
			public static StiArabicDigitsType ArabicDigitsType { get; set; } = StiArabicDigitsType.Standard;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to reduce font file size.")]
			[StiSerializable]
			public static bool ReduceFontFileSize { get; set; } = true;


			[DefaultValue(false)]
			[StiSerializable]
			[Obsolete("AllowEditablePdf property is obsolete. Please use StiPdfExportSettings instead.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool AllowEditablePdf { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			public static bool UseEditableFieldName { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			public static bool UseEditableFieldAlias { get; set; }

			[DefaultValue(false)]
			[StiSerializable]
			public static bool UseEditableFieldTag { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use image comparer.")]
			[StiSerializable]
			public static bool AllowImageComparer { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use image transparency.")]
			[StiSerializable]
			public static bool AllowImageTransparency { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use Printable property of the component.")]
			[StiSerializable]
			public static bool AllowPrintable { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether it is necessary to allow inherited page resources.")]
			[StiSerializable]
			public static bool AllowInheritedPageResources { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to allow using of ExtGState.")]
			[StiSerializable]
			public static bool AllowExtGState { get; set; } = true;


			[DefaultValue("")]
			[Description("Gets or sets a CreatorString.")]
			[StiSerializable]
			public static string CreatorString { get; set; } = string.Empty;


			[DefaultValue("")]
			[Description("Gets or sets a KeywordsString.")]
			[StiSerializable]
			public static string KeywordsString { get; set; } = string.Empty;


			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether it is necessary to use fonts cache.")]
			[StiSerializable]
			public static bool AllowFontsCache { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets a value indicating the possibility to use the system libraries for working with fonts.")]
			[StiSerializable]
			public static bool AllowImportSystemLibraries { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether it is necessary to force the use of the FontsInfoStore for Wysiwyg mode.")]
			[StiSerializable]
			public static bool ForceUseFontsInfoStoreForWysiwyg { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether it is necessary to use old mode of the AllowHtmlTags rendering.")]
			[StiSerializable]
			public static bool UseOldModeAllowHtmlTags { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use wysiwyg render.")]
			[StiSerializable]
			public static bool UseWysiwygRender { get; set; } = true;


			[DefaultValue(2)]
			[Description("Gets or sets a value indicating default coordinates precision.")]
			[StiSerializable]
			public static int DefaultCoordinatesPrecision { get; set; } = 2;


			[DefaultValue(StiPdfAutoPrintMode.None)]
			[Description("Gets or sets a value indicating default AutoPrint mode.")]
			[StiSerializable]
			public static StiPdfAutoPrintMode DefaultAutoPrintMode { get; set; } = StiPdfAutoPrintMode.None;


			[Obsolete("WpfHighAccuracyWordWrap property is obsolete. Please use WpfHighAccuracyMode instead.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool WpfHighAccuracyWordWrap
			{
				get
				{
					return WpfHighAccuracyMode == StiPdfHighAccuracyMode.All;
				}
				set
				{
					WpfHighAccuracyMode = StiPdfHighAccuracyMode.All;
				}
			}

			[Obsolete("WinFormsHighAccuracyWordWrap property is obsolete. Please use WinFormsHighAccuracyMode instead.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool WinFormsHighAccuracyWordWrap
			{
				get
				{
					return WinFormsHighAccuracyMode == StiPdfHighAccuracyMode.All;
				}
				set
				{
					WinFormsHighAccuracyMode = StiPdfHighAccuracyMode.All;
				}
			}

			[DefaultValue(StiPdfHighAccuracyMode.No)]
			[Description("Gets or sets a value indicating whether it is necessary to use high accuracy in Wpf mode.")]
			[StiSerializable]
			public static StiPdfHighAccuracyMode WpfHighAccuracyMode { get; set; } = StiPdfHighAccuracyMode.No;


			[DefaultValue(StiPdfHighAccuracyMode.No)]
			[Description("Gets or sets a value indicating whether it is necessary to use high accuracy in WinForms mode.")]
			[StiSerializable]
			public static StiPdfHighAccuracyMode WinFormsHighAccuracyMode { get; set; } = StiPdfHighAccuracyMode.No;


			[DefaultValue(null)]
			[Description("Gets or sets a value indicating the Reason field of digital signature.")]
			[StiSerializable]
			public static string DigitalSignatureReason { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating the Location field of digital signature.")]
			[StiSerializable]
			public static string DigitalSignatureLocation { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating the ContactInfo field of digital signature.")]
			[StiSerializable]
			public static string DigitalSignatureContactInfo { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating the SignedBy field of digital signature.")]
			[StiSerializable]
			public static string DigitalSignatureSignedBy { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating the digest used in the digital signature.")]
			[StiSerializable]
			public static bool DigitalSignatureDigestSHA256 { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether it is necessary to disable DigitalSignature button in the export menu.")]
			[StiSerializable]
			public static bool DisableDigitalSignatureButtonInExportMenu { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets a value indicating a BarCode rendering mode.")]
			[StiSerializable]
			public static bool RenderBarCodeAsImage { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether it is necessary to order components by placement.")]
			[StiSerializable]
			public static bool OrderComponentsByPlacement { get; set; }

			[DefaultValue(0.95)]
			[Description("Gets or sets a value of vertical scaling for RichText in Wpf mode.")]
			[StiSerializable]
			public static float WpfRichTextVerticalScaling { get; set; } = 0.95f;

		}

		public class PowerPoint
		{
			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use image comparer.")]
			[StiSerializable]
			public static bool AllowImageComparer { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to store images in PNG format instead of JPG.")]
			[StiSerializable]
			public static bool StoreImagesAsPng { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to replace transparent page background with white color.")]
			[StiSerializable]
			public static bool ReplaceTransparentPageBackground { get; set; } = true;

		}

		[Obsolete("The class StiOptions.Ppt2007 is obsolete! Please use class StiOptions.PowerPoint instead it.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class Ppt2007 : PowerPoint
		{
		}

		public class RichText
		{
			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use PAGEREF field of MS-Word.")]
			[StiSerializable]
			public static bool UsePageRefField { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value, which indicates whether it is necessary to convert all digits to arabic digits.")]
			[StiSerializable]
			public static bool ConvertDigitsToArabic { get; set; }

			[DefaultValue(StiArabicDigitsType.Standard)]
			[Description("Gets or sets a value indicating a current arabic type of digits.")]
			[StiSerializable]
			public static StiArabicDigitsType ArabicDigitsType { get; set; } = StiArabicDigitsType.Standard;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating behavior of the exporting segmented pages will be forcibly broken in order of chosen page format.")]
			[StiSerializable]
			public static bool DivideSegmentPages { get; set; } = true;


			[DefaultValue(false)]
			[Description("When Rtf exporting segmented pages will be forcibly use the page size from template. This property work only if DivideSegmentPages=false.")]
			[StiSerializable]
			public static bool UseTemplatePageSize { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating a line height mode - 'exactly' or 'at least'.")]
			[StiSerializable]
			public static bool LineHeightExactly { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value indicating a forcing of line height.")]
			[StiSerializable]
			public static bool ForceLineHeight { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to remove empty space at the bottom of page.")]
			[StiSerializable]
			public static bool RemoveEmptySpaceAtBottom { get; set; } = true;


			[DefaultValue(0.922)]
			[Description("Gets or sets a value of line spacing.")]
			[StiSerializable]
			public static double LineSpacing { get; set; } = 0.922;


			[DefaultValue(0)]
			[Description("Gets or sets a value of right margin correction.")]
			[StiSerializable]
			public static int RightMarginCorrection { get; set; }

			[DefaultValue(42)]
			[Description("Gets or sets a value of bottom margin correction.")]
			[StiSerializable]
			public static int BottomMarginCorrection { get; set; } = 42;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating behavior of the exporting big cells will be divided into small cells.")]
			[StiSerializable]
			public static bool DivideBigCells { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use CanBreak property of components.")]
			[StiSerializable]
			public static bool UseCanBreakProperty { get; set; } = true;


			[DefaultValue(-2)]
			[Description("Gets or sets a value of expansion or compression of the space between characters in twips; a negative value compresses.")]
			[StiSerializable]
			public static int SpaceBetweenCharacters { get; set; } = -2;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating a forcing of empty cells optimization.")]
			[StiSerializable]
			public static bool ForceEmptyCellsOptimization { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value indicating whether it is necessary to use NewPage command instead of NewSection.")]
			[StiSerializable]
			public static bool UseNewPageCommandInsteadOfNewSection { get; set; }
		}

		[Obsolete("The class StiOptions.Rtf is obsolete! Please use class StiOptions.RichText instead it.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class Rtf : RichText
		{
		}

		public class Text
		{
			private static List<StiEscapeCodesCollection> escapeCodesCollectionList;

			[Obsolete("ColumnWidths property is obsolete.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			[DefaultValue("")]
			[Description("Set the columns width in format 'a;b;c;d;e;f; ... '.")]
			[StiSerializable]
			public static string ColumnWidths { get; set; } = string.Empty;


			[DefaultValue(false)]
			[Description("Gets or sets a value which indicates whether it is necessary to use full width of TextBox to put text.")]
			[StiSerializable]
			public static bool UseFullTextBoxWidth { get; set; }

			[Obsolete("UseOldMode property is obsolete.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			[DefaultValue(false)]
			[Description("Gets or sets a value which indicates whether it is necessary to use old export mode.")]
			[StiSerializable]
			public static bool UseOldMode { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value which indicates whether it is necessary to use full  vertical border.")]
			[StiSerializable]
			public static bool UseFullVerticalBorder { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value which indicates whether it is necessary to use full horizontal border.")]
			[StiSerializable]
			public static bool UseFullHorizontalBorder { get; set; } = true;


			[DefaultValue("+")]
			[Description("Gets or sets a text of checkbox for true.")]
			[StiSerializable]
			public static string CheckBoxTextForTrue { get; set; } = "+";


			[DefaultValue("-")]
			[Description("Gets or sets a text of checkbox for true.")]
			[StiSerializable]
			public static string CheckBoxTextForFalse { get; set; } = "-";


			[DefaultValue(true)]
			[Description("Gets or sets a value which indicates whether it is necessary to trim trailing spaces.")]
			[StiSerializable]
			public static bool TrimTrailingSpaces { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value which indicates whether it is necessary to remove last NewLine marker.")]
			[StiSerializable]
			public static bool RemoveLastNewLineMarker { get; set; }

			public static List<StiEscapeCodesCollection> EscapeCodesCollectionList
			{
				get
				{
					if (escapeCodesCollectionList == null)
					{
						escapeCodesCollectionList = new List<StiEscapeCodesCollection>();
						StiEscapeCodesCollection stiEscapeCodesCollection = new StiEscapeCodesCollection();
						stiEscapeCodesCollection.Name = "EpsonFX";
						stiEscapeCodesCollection["b"] = "\u001bE";
						stiEscapeCodesCollection["/b"] = "\u001bF";
						stiEscapeCodesCollection["i"] = "\u001b4";
						stiEscapeCodesCollection["/i"] = "\u001b5";
						stiEscapeCodesCollection["u"] = "\u001b-1";
						stiEscapeCodesCollection["/u"] = "\u001b-0";
						stiEscapeCodesCollection["sup"] = "\u001bS0";
						stiEscapeCodesCollection["/sup"] = "\u001bT";
						stiEscapeCodesCollection["sub"] = "\u001bS1";
						stiEscapeCodesCollection["/sub"] = "\u001bT";
						stiEscapeCodesCollection["condensed"] = "\u000f";
						stiEscapeCodesCollection["/condensed"] = "\u0012";
						stiEscapeCodesCollection["elite"] = "\u001bM";
						stiEscapeCodesCollection["pica"] = "\u001bP";
						stiEscapeCodesCollection["doublewidth"] = "\u001bW1";
						stiEscapeCodesCollection["/doublewidth"] = "\u001bW0";
						stiEscapeCodesCollection["ff"] = "\f";
						StiEscapeCodesCollection stiEscapeCodesCollection2 = new StiEscapeCodesCollection();
						stiEscapeCodesCollection2.Name = "Oki ML92/93";
						stiEscapeCodesCollection2["b"] = "\u001bT";
						stiEscapeCodesCollection2["/b"] = "\u001bI";
						stiEscapeCodesCollection2["u"] = "\u001bH";
						stiEscapeCodesCollection2["/u"] = "\u001bD";
						stiEscapeCodesCollection2["sup"] = "\u001bJ";
						stiEscapeCodesCollection2["/sup"] = "\u001bK";
						stiEscapeCodesCollection2["sub"] = "\u001bL";
						stiEscapeCodesCollection2["/sub"] = "\u001bM";
						stiEscapeCodesCollection2["condensed"] = "\u001d";
						stiEscapeCodesCollection2["/condensed"] = "\u001e";
						stiEscapeCodesCollection2["elite"] = "\u001c";
						stiEscapeCodesCollection2["pica"] = "\u001e";
						stiEscapeCodesCollection2["doublewidth"] = "\u001f";
						stiEscapeCodesCollection2["/doublewidth"] = "\u001e";
						stiEscapeCodesCollection2["ff"] = "\f";
						StiEscapeCodesCollection stiEscapeCodesCollection3 = new StiEscapeCodesCollection();
						stiEscapeCodesCollection3.Name = "ESC/POS";
						stiEscapeCodesCollection3["b"] = "\u001bE\u0001";
						stiEscapeCodesCollection3["/b"] = "\u001bE\0";
						stiEscapeCodesCollection3["u"] = "\u001b-\u0001";
						stiEscapeCodesCollection3["/u"] = "\u001b-\0";
						stiEscapeCodesCollection3["doublewidth"] = "\u001b! ";
						stiEscapeCodesCollection3["/doublewidth"] = "\u001b!\0";
						stiEscapeCodesCollection3["ff"] = "\u001bi";
						StiEscapeCodesCollection stiEscapeCodesCollection4 = new StiEscapeCodesCollection();
						stiEscapeCodesCollection4.Name = "None";
						stiEscapeCodesCollection4["ff"] = "\f";
						escapeCodesCollectionList.Add(stiEscapeCodesCollection4);
						escapeCodesCollectionList.Add(stiEscapeCodesCollection);
						escapeCodesCollectionList.Add(stiEscapeCodesCollection2);
						escapeCodesCollectionList.Add(stiEscapeCodesCollection3);
					}
					return escapeCodesCollectionList;
				}
				set
				{
					escapeCodesCollectionList = value;
				}
			}
		}

		[Obsolete("The class StiOptions.Txt is obsolete! Please use class StiOptions.Text instead it.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class Txt : Text
		{
		}

		public class Word
		{
			[DefaultValue(true)]
			[Description("Gets or sets a value indicating behavior of the exporting segmented pages will be forcibly broken in order of chosen page format.")]
			[StiSerializable]
			public static bool DivideSegmentPages { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use image comparer.")]
			[StiSerializable]
			public static bool AllowImageComparer { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to remove empty space at the bottom of page.")]
			public static bool RemoveEmptySpaceAtBottom { get; set; } = true;


			[DefaultValue(-2)]
			[Description("Gets or sets a value of expansion or compression of the space between characters in twips; a negative value compresses.")]
			[StiSerializable]
			public static int SpaceBetweenCharacters { get; set; } = -2;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating a line height mode - 'exactly' or 'at least'.")]
			[StiSerializable]
			public static bool LineHeightExactly { get; set; } = true;


			[DefaultValue(false)]
			[Description("Gets or sets a value indicating a line height mode - 'exactly' or 'at least' for 'UsePageHeadersAndFooters' mode.")]
			[StiSerializable]
			public static bool LineHeightExactlyForPHFMode { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating a forcing of line height.")]
			[StiSerializable]
			public static bool ForceLineHeight { get; set; } = true;


			[DefaultValue(0)]
			[Description("Gets or sets a value of right margin correction.")]
			[StiSerializable]
			public static int RightMarginCorrection { get; set; }

			[DefaultValue(50)]
			[Description("Gets or sets a value of bottom margin correction.")]
			[StiSerializable]
			public static int BottomMarginCorrection { get; set; } = 50;


			[DefaultValue(false)]
			[Description("Gets or sets a value indicating a RichText rendering mode.")]
			[StiSerializable]
			public static bool RenderRichTextAsImage { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets a value indicating a Html tags rendering mode.")]
			[StiSerializable]
			public static bool RenderHtmlTagsAsImage { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets a value indicating a Html tags rendering mode.")]
			[StiSerializable]
			public static bool RenderHtmlTagsAsEmbeddedRichText { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets a value indicating a checkbox rendering mode.")]
			[StiSerializable]
			public static bool RenderCheckBoxAsImage { get; set; }

			[DefaultValue(true)]
			[Description("This value indicates that it is necessary to correct problem with FontSize=11 in embedded RichTexts.")]
			[StiSerializable]
			public static bool AllowCorrectFontSize11Problem { get; set; } = true;


			[DefaultValue(0.95)]
			[Description("Gets or sets a value of line spacing correction in embedded RichTexts.")]
			[StiSerializable]
			public static double RichTextLineSpacingCorrection { get; set; } = 0.95;


			[DefaultValue(1f)]
			[Description("Gets or sets a value of default FontSize for the Normal style.")]
			[StiSerializable]
			public static float NormalStyleDefaultFontSize { get; set; } = 1f;


			[DefaultValue(0.955)]
			[Description("Gets or sets a value of line spacing correction.")]
			[StiSerializable]
			public static double LineSpacing { get; set; } = 0.955;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating behavior of the exporting big cells will be divided into small cells.")]
			[StiSerializable]
			public static bool DivideBigCells { get; set; } = true;


			[DefaultValue(15)]
			[Description("Gets or sets a value indicating version of the document compatibility mode.")]
			[StiSerializable]
			public static int CompatibilityModeValue { get; set; } = 15;


			[DefaultValue(StiWord2007RestrictEditing.No)]
			[StiSerializable]
			public static StiWord2007RestrictEditing RestrictEditing { get; set; } = StiWord2007RestrictEditing.No;

		}

		[Obsolete("The class StiOptions.Word2007 is obsolete! Please use class StiOptions.Word instead it.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class Word2007 : Word
		{
		}

		public sealed class Xml
		{
			[DefaultValue(false)]
			[Description("Gets or sets a value which indicates whether it is necessary to use aliases instead of names of components for columns name.")]
			[StiSerializable]
			public static bool UseAliases { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value which indicates whether it is necessary to write Xml Document Declaration.")]
			[StiSerializable]
			public static bool WriteXmlDocumentDeclaration { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value which indicates whether it is necessary to convert tags to upperCase.")]
			[StiSerializable]
			public static bool ConvertTagsToUpperCase { get; set; } = true;

		}

		public sealed class Xps
		{
			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use image comparer.")]
			[StiSerializable]
			public static bool AllowImageComparer { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to use image transparency.")]
			[StiSerializable]
			public static bool AllowImageTransparency { get; set; } = true;


			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether it is necessary to reduce font file size.")]
			[StiSerializable]
			public static bool ReduceFontFileSize { get; set; } = true;

		}

		private static Hashtable _alternativeFontNames = null;

		[DefaultValue("true")]
		[Description("Gets or sets a text of checkbox for true.")]
		[StiSerializable]
		public static string CheckBoxTextForTrue { get; set; } = "false";


		[DefaultValue("false")]
		[Description("Gets or sets a text of checkbox for true.")]
		[StiSerializable]
		public static string CheckBoxTextForFalse { get; set; } = "true";


		[DefaultValue(false)]
		[Description("Gets or sets value which indicates that file name for Send EMail function will be generated automatically.")]
		[StiSerializable]
		public static bool AutoGenerateFileNameInSendEMail { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value indicating whether it is necessary to disable ClearType feature during export.")]
		[StiSerializable]
		public static bool DisableClearTypeDuringExport { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value indicating whether it is necessary to use cache mode for the StiMatrix.")]
		[StiSerializable]
		public static bool UseCacheModeForStiMatrix { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value indicating whether it is necessary to optimize exports in DataOnly mode.")]
		[StiSerializable]
		public static bool OptimizeDataOnlyMode { get; set; } = true;


		[DefaultValue(StiImageCutEdgesMode.ExceptFeed)]
		[Description("Gets or sets a value indicating the image CutEdges mode.")]
		[StiSerializable]
		public static StiImageCutEdgesMode ImageCutEdgesMode { get; set; } = StiImageCutEdgesMode.ExceptFeed;


		[DefaultValue(true)]
		[Description("Gets or sets a value indicating whether it is necessary to use alternative font names.")]
		[StiSerializable]
		public static bool UseAlternativeFontNames { get; set; } = true;


		public static Hashtable AlternativeFontNames
		{
			get
			{
				if (_alternativeFontNames == null)
				{
					_alternativeFontNames = new Hashtable
					{
						{ "ＭＳ Ｐゴシック", "MS PGothic" },
						{ "ＭＳ Ｐ明朝", "MS PMincho" },
						{ "ＭＳ ゴシック", "MS Gothic" },
						{ "ＭＳ 明朝", "MS Mincho" },
						{ "メイリオ", "Meiryo" },
						{ "바탕", "Batang" },
						{ "바탕체", "Batangche" },
						{ "굴림", "Gulim" },
						{ "굴림체", "Gulimche" },
						{ "돋움", "Dotum" },
						{ "돋움체", "Dotumche" },
						{ "궁서", "Gungsuh" },
						{ "궁서체", "Gungsuhche" },
						{ "맑은 고딕", "Malgun Gothic" },
						{ "宋体", "SimSun" },
						{ "宋体-ExtB", "SimSun-ExtB" },
						{ "黑体", "SimHei" },
						{ "新宋体", "NSimSun" },
						{ "微软雅黑", "Microsoft YaHei" },
						{ "仿宋", "FangSong" },
						{ "楷体", "KaiTi" },
						{ "仿宋_GB2312", "FangSong_GB2312" },
						{ "新細明體", "PMingLiU" },
						{ "新細明體-ExtB", "PMingLiU-ExtB" },
						{ "細明體", "MingLiU" },
						{ "細明體-ExtB", "MingLiU-ExtB" },
						{ "微軟正黑體", "Microsoft JhengHei" },
						{ "標楷體", "DFKai-SB" },
						{ "文泉驛正黑", "WenQuanYi Zen Hei" },
						{ "文泉驿正黑", "WenQuanYi Zen Hei" },
						{ "文鼎PL細上海宋Uni", "AR PL ShanHeiSun Uni" },
						{ "文鼎PL细上海宋Uni", "AR PL ShanHeiSun Uni" },
						{ "文PL中楷Uni", "AR PL ZenKai Uni" },
						{ "細曠體_HKSCS", "MingLiU_HKSCS" },
						{ "細曠體_HKSCS-ExtB", "MingLiU_HKSCS-ExtB" }
					};
				}
				return _alternativeFontNames;
			}
		}
	}

	public sealed class Localization
	{
		private static bool isLoaded;

		public static void Load(string file)
		{
			isLoaded = false;
			StiLocalization.Load(file);
			isLoaded = true;
		}

		public static void Load(Stream stream)
		{
			isLoaded = false;
			StiLocalization.Load(stream);
			isLoaded = true;
		}

		public static string GetDirectory()
		{
			string directoryLocalizationFromSettings = GetDirectoryLocalizationFromSettings();
			if (directoryLocalizationFromSettings != null)
			{
				return directoryLocalizationFromSettings;
			}
			directoryLocalizationFromSettings = StiLocalization.GetDirectoryLocales();
			if (directoryLocalizationFromSettings != null)
			{
				return directoryLocalizationFromSettings;
			}
			directoryLocalizationFromSettings = StiLocalization.GetDirectoryLocalizationFromRegistry();
			if (directoryLocalizationFromSettings != null)
			{
				return directoryLocalizationFromSettings;
			}
			return Configuration.DirectoryLocalization;
		}

		private static string GetDirectoryLocalizationFromSettings()
		{
			try
			{
				StiSettings.Load();
				return StiSettings.GetStr("Localization", "DirectoryLocalization", null);
			}
			catch
			{
			}
			return null;
		}

		public static bool TryLoadDefault()
		{
			try
			{
				LoadDefault();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static void LoadDefault()
		{
			if (Configuration.IsServer)
			{
				return;
			}
			StiSettings.Load();
			if (!isLoaded)
			{
				string directory = GetDirectory();
				string text = StiSettings.GetStr("Localization", "FileName", null);
				if (text == null)
				{
					text = Configuration.Localization;
				}
				StiLocalization.DirectoryLocalization = directory;
				StiLocalization.Localization = text;
				StiLocalization.LoadCurrentLocalization();
				isLoaded = true;
			}
		}
	}

	public sealed class Print
	{
		[DefaultValue(false)]
		[Description("Gets or sets a value which indicating that printer settings will be stored in report.PrinterSetting property after printer dialog.")]
		[StiSerializable]
		public static bool StorePrinterSettingsInReportAfterPrintDialog { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value indicating whether to render charts as Bitmap.")]
		[StiSerializable]
		public static bool ChartAsBitmap { get; set; } = true;


		[DefaultValue(false)]
		[Description("Gets or sets a value which indicating that PaperSize and PaperSource from PrintDialog will be used for all pages if they changed in the PrintDialog.")]
		[StiSerializable]
		public static bool UsePaperSizeAndSourceFromPrintDialogIfTheyChanged { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets a value which indicating that PaperSize and PaperSource from PrintDialog will be used for all pages always.")]
		[StiSerializable]
		public static bool UsePaperSizeAndSourceFromPrintDialogAlways { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets a value which indicating that PrinterSettings must be used entirely.")]
		[StiSerializable]
		public static bool UsePrinterSettingsEntirely { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value which force to find the custom PaperSize in the standard PaperSizes.")]
		[StiSerializable]
		public static bool FindCustomPaperSizeInStandardPaperSizes { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets a value which force to use PaperSizes collection from PrinterSettings.")]
		[StiSerializable]
		public static bool AllowUsePaperSizesFromPrinterSettings { get; set; } = true;


		[DefaultValue(/*Could not decode attribute arguments.*/)]
		[Description("Gets or sets the page horizontal alignment.")]
		[StiSerializable]
		public static StiHorAlignment PageHorAlignment { get; set; } = (StiHorAlignment)1;


		[DefaultValue(/*Could not decode attribute arguments.*/)]
		[Description("Gets or sets the page vertical alignment.")]
		[StiSerializable]
		public static StiVertAlignment PageVertAlignment { get; set; } = (StiVertAlignment)0;


		[DefaultValue(false)]
		[Description("Gets or sets a value indicating whether to render metafile as Bitmap.")]
		[StiSerializable]
		public static bool MetafileAsBitmap { get; set; }

		[DefaultValue(true)]
		[Description("Gets or sets a value indicating whether to fix problem with invariant culture when printing in wpf.")]
		[StiSerializable]
		public static bool FixNet46WpfCulture { get; set; } = true;


		[DefaultValue(null)]
		[StiSerializable]
		public static string DotMatrixFormatSequence { get; set; }

		public static PaperSizeCollection CustomPaperSizes { get; set; }

		public static PaperSourceCollection CustomPaperSources { get; set; }

		[DefaultValue(false)]
		[Description("Gets or sets a value indicating whether to fix the banded gradients on some printers.")]
		[StiSerializable]
		public static bool FixBandedGradients
		{
			get
			{
				return StiBaseOptions.FixBandedGradients;
			}
			set
			{
				StiBaseOptions.FixBandedGradients = value;
			}
		}

		[DefaultValue(false)]
		[Description("Gets or sets a value indicating whether to render barcodes as Bitmap.")]
		[StiSerializable]
		public static bool BarcodeAsBitmap { get; set; }
	}

	public sealed class Services
	{
		public static class Dashboards
		{
			private static List<StiDashboardStyle> dashboardStyles;

			private static List<StiControlElementStyle> controlStyles;

			private static List<StiIndicatorElementStyle> indicatorStyles;

			private static List<StiPivotElementStyle> pivotStyles;

			private static List<StiProgressElementStyle> progressStyles;

			private static List<StiTableElementStyle> tableStyles;

			private static List<StiCardsElementStyle> cardsStyles;

			public static List<StiDashboardStyle> DashboardStyles
			{
				get
				{
					lock (lockObject)
					{
						object obj = dashboardStyles;
						if (obj == null)
						{
							obj = new List<StiDashboardStyle>
							{
								new StiBlueDashboardStyle(),
								new StiOrangeDashboardStyle(),
								new StiGreenDashboardStyle(),
								new StiTurquoiseDashboardStyle(),
								new StiSlateGrayDashboardStyle(),
								new StiDarkBlueDashboardStyle(),
								new StiDarkGrayDashboardStyle(),
								new StiDarkTurquoiseDashboardStyle(),
								new StiSilverDashboardStyle(),
								new StiAliceBlueDashboardStyle(),
								new StiDarkGreenDashboardStyle(),
								new StiSiennaDashboardStyle()
							};
							dashboardStyles = (List<StiDashboardStyle>)obj;
						}
						return (List<StiDashboardStyle>)obj;
					}
				}
			}

			public static List<StiControlElementStyle> ControlStyles
			{
				get
				{
					lock (lockObject)
					{
						object obj = controlStyles;
						if (obj == null)
						{
							obj = new List<StiControlElementStyle>
							{
								new StiBlueControlElementStyle(),
								new StiOrangeControlElementStyle(),
								new StiGreenControlElementStyle(),
								new StiTurquoiseControlElementStyle(),
								new StiSlateGrayControlElementStyle(),
								new StiDarkBlueControlElementStyle(),
								new StiDarkGrayControlElementStyle(),
								new StiDarkTurquoiseControlElementStyle(),
								new StiSilverControlElementStyle(),
								new StiAliceBlueControlElementStyle(),
								new StiDarkGreenControlElementStyle(),
								new StiSiennaControlElementStyle()
							};
							controlStyles = (List<StiControlElementStyle>)obj;
						}
						return (List<StiControlElementStyle>)obj;
					}
				}
			}

			public static List<StiIndicatorElementStyle> IndicatorStyles
			{
				get
				{
					lock (lockObject)
					{
						object obj = indicatorStyles;
						if (obj == null)
						{
							obj = new List<StiIndicatorElementStyle>
							{
								new StiBlueIndicatorElementStyle(),
								new StiOrangeIndicatorElementStyle(),
								new StiGreenIndicatorElementStyle(),
								new StiTurquoiseIndicatorElementStyle(),
								new StiSlateGrayIndicatorElementStyle(),
								new StiDarkBlueIndicatorElementStyle(),
								new StiDarkGrayIndicatorElementStyle(),
								new StiDarkTurquoiseIndicatorElementStyle(),
								new StiSilverIndicatorElementStyle(),
								new StiAliceBlueIndicatorElementStyle(),
								new StiDarkGreenIndicatorElementStyle(),
								new StiSiennaIndicatorElementStyle()
							};
							indicatorStyles = (List<StiIndicatorElementStyle>)obj;
						}
						return (List<StiIndicatorElementStyle>)obj;
					}
				}
			}

			public static List<StiPivotElementStyle> PivotStyles
			{
				get
				{
					lock (lockObject)
					{
						object obj = pivotStyles;
						if (obj == null)
						{
							obj = new List<StiPivotElementStyle>
							{
								new StiBluePivotElementStyle(),
								new StiOrangePivotElementStyle(),
								new StiGreenPivotElementStyle(),
								new StiTurquoisePivotElementStyle(),
								new StiSlateGrayPivotElementStyle(),
								new StiDarkBluePivotElementStyle(),
								new StiDarkGrayPivotElementStyle(),
								new StiDarkTurquoisePivotElementStyle(),
								new StiSilverPivotElementStyle(),
								new StiAliceBluePivotElementStyle(),
								new StiDarkGreenPivotElementStyle(),
								new StiSiennaPivotElementStyle()
							};
							pivotStyles = (List<StiPivotElementStyle>)obj;
						}
						return (List<StiPivotElementStyle>)obj;
					}
				}
			}

			public static List<StiProgressElementStyle> ProgressStyles
			{
				get
				{
					lock (lockObject)
					{
						object obj = progressStyles;
						if (obj == null)
						{
							obj = new List<StiProgressElementStyle>
							{
								new StiBlueProgressElementStyle(),
								new StiOrangeProgressElementStyle(),
								new StiGreenProgressElementStyle(),
								new StiTurquoiseProgressElementStyle(),
								new StiSlateGrayProgressElementStyle(),
								new StiDarkBlueProgressElementStyle(),
								new StiDarkGrayProgressElementStyle(),
								new StiDarkTurquoiseProgressElementStyle(),
								new StiSilverProgressElementStyle(),
								new StiAliceBlueProgressElementStyle(),
								new StiDarkGreenProgressElementStyle(),
								new StiSiennaProgressElementStyle()
							};
							progressStyles = (List<StiProgressElementStyle>)obj;
						}
						return (List<StiProgressElementStyle>)obj;
					}
				}
			}

			public static List<StiTableElementStyle> TableStyles
			{
				get
				{
					lock (lockObject)
					{
						object obj = tableStyles;
						if (obj == null)
						{
							obj = new List<StiTableElementStyle>
							{
								new StiBlueTableElementStyle(),
								new StiOrangeTableElementStyle(),
								new StiGreenTableElementStyle(),
								new StiTurquoiseTableElementStyle(),
								new StiSlateGrayTableElementStyle(),
								new StiDarkBlueTableElementStyle(),
								new StiDarkGrayTableElementStyle(),
								new StiDarkTurquoiseTableElementStyle(),
								new StiSilverTableElementStyle(),
								new StiAliceBlueTableElementStyle(),
								new StiDarkGreenTableElementStyle(),
								new StiSiennaTableElementStyle()
							};
							tableStyles = (List<StiTableElementStyle>)obj;
						}
						return (List<StiTableElementStyle>)obj;
					}
				}
			}

			public static List<StiCardsElementStyle> CardsStyles
			{
				get
				{
					lock (lockObject)
					{
						object obj = cardsStyles;
						if (obj == null)
						{
							obj = new List<StiCardsElementStyle>
							{
								new StiBlueCardsElementStyle(),
								new StiOrangeCardsElementStyle(),
								new StiGreenCardsElementStyle(),
								new StiTurquoiseCardsElementStyle(),
								new StiSlateGrayCardsElementStyle(),
								new StiDarkBlueCardsElementStyle(),
								new StiDarkGrayCardsElementStyle(),
								new StiDarkTurquoiseCardsElementStyle(),
								new StiSilverCardsElementStyle(),
								new StiAliceBlueCardsElementStyle(),
								new StiDarkGreenCardsElementStyle(),
								new StiSiennaCardsElementStyle()
							};
							cardsStyles = (List<StiCardsElementStyle>)obj;
						}
						return (List<StiCardsElementStyle>)obj;
					}
				}
			}
		}

		private static List<StiAggregateFunctionService> aggregateFunctions;

		private static List<StiBarCodeTypeService> barCodes;

		private static List<StiArea> chartAreas;

		private static List<StiSeriesLabels> chartSerieLabels;

		private static List<StiSeries> chartSeries;

		private static List<Stimulsoft.Report.Chart.StiChartStyle> chartStyles;

		private static List<StiTrendLine> chartTrendLines;

		private static List<StiComponent> components;

		private static List<StiCustomValueBase> customValues;

		private static List<StiDataAdapterService> dataAdapters;

		private static List<StiDatabase> databases;

		private static List<StiDataSource> dataSource;

		private static List<StiDataTableSetNameService> dataTableSetName;

		private static List<StiDialogsProvider> dialogProviders;

		private static List<StiDictionarySLService> dictionarySLs;

		private static List<StiDocumentSLService> documentSLs;

		private static List<StiDotMatrixViewerConfigService> dotMatrixViewerConfig;

		private static List<StiExportService> exports;

		private static List<StiFormatService> formats;

		private static List<StiGaugeElement> gaugeElements;

		private static List<StiScaleBase> gaugeScales;

		private static List<StiGaugeStyleXF> gaugeStyles;

		private static List<StiIndicatorRangeInfo> indicatorRanges;

		private static List<StiLogService> logs;

		private static List<StiMapStyleFX> mapStyles;

		private static List<StiPageSLService> pageSLs;

		private static List<StiRangeBase> ranges;

		private static List<StiReportSLService> reportSLs;

		private static List<StiShapeTypeService> shapes;

		private static List<StiTableStyleFX> tableStyles;

		private static List<StiViewerConfigService> viewerConfig;

		private static object lockObject = new object();

		public static List<StiAggregateFunctionService> AggregateFunctions
		{
			get
			{
				lock (lockObject)
				{
					object obj = aggregateFunctions;
					if (obj == null)
					{
						obj = new List<StiAggregateFunctionService>
						{
							new StiAvgDecimalFunctionService(),
							new StiAvgDoubleFunctionService(),
							new StiAvgIntFunctionService(),
							new StiAvgDateFunctionService(),
							new StiAvgTimeFunctionService(),
							new StiCountDistinctFunctionService(),
							new StiCountFunctionService(),
							new StiFirstFunctionService(),
							new StiLastFunctionService(),
							new StiMaxDecimalFunctionService(),
							new StiMaxDoubleFunctionService(),
							new StiMaxIntFunctionService(),
							new StiMaxDateFunctionService(),
							new StiMaxTimeFunctionService(),
							new StiMaxStrFunctionService(),
							new StiMinDecimalFunctionService(),
							new StiMinDoubleFunctionService(),
							new StiMinIntFunctionService(),
							new StiMinDateFunctionService(),
							new StiMinTimeFunctionService(),
							new StiMinStrFunctionService(),
							new StiMedianDecimalFunctionService(),
							new StiMedianDoubleFunctionService(),
							new StiMedianIntFunctionService(),
							new StiModeDecimalFunctionService(),
							new StiModeDoubleFunctionService(),
							new StiModeIntFunctionService(),
							new StiSumDecimalFunctionService(),
							new StiSumDoubleFunctionService(),
							new StiSumIntFunctionService(),
							new StiSumTimeFunctionService(),
							new StiSumDistinctDecimalFunctionService()
						};
						aggregateFunctions = (List<StiAggregateFunctionService>)obj;
					}
					return (List<StiAggregateFunctionService>)obj;
				}
			}
		}

		public static List<StiBarCodeTypeService> BarCodes
		{
			get
			{
				lock (lockObject)
				{
					object obj = barCodes;
					if (obj == null)
					{
						obj = new List<StiBarCodeTypeService>
						{
							new StiQRCodeBarCodeType(),
							new StiDataMatrixBarCodeType(),
							new StiMaxicodeBarCodeType(),
							new StiPdf417BarCodeType(),
							new StiAztecBarCodeType(),
							new StiAustraliaPost4StateBarCodeType(),
							new StiIntelligentMail4StateBarCodeType(),
							new StiPharmacodeBarCodeType(),
							new StiCode11BarCodeType(),
							new StiCode128aBarCodeType(),
							new StiCode128bBarCodeType(),
							new StiCode128cBarCodeType(),
							new StiCode128AutoBarCodeType(),
							new StiCode39BarCodeType(),
							new StiCode39ExtBarCodeType(),
							new StiCode93BarCodeType(),
							new StiCode93ExtBarCodeType(),
							new StiCodabarBarCodeType(),
							new StiEAN128aBarCodeType(),
							new StiEAN128bBarCodeType(),
							new StiEAN128cBarCodeType(),
							new StiEAN128AutoBarCodeType(),
							new StiGS1_128BarCodeType(),
							new StiEAN13BarCodeType(),
							new StiEAN8BarCodeType(),
							new StiFIMBarCodeType(),
							new StiIsbn10BarCodeType(),
							new StiIsbn13BarCodeType(),
							new StiITF14BarCodeType(),
							new StiJan13BarCodeType(),
							new StiJan8BarCodeType(),
							new StiMsiBarCodeType(),
							new StiPlesseyBarCodeType(),
							new StiPostnetBarCodeType(),
							new StiDutchKIXBarCodeType(),
							new StiRoyalMail4StateBarCodeType(),
							new StiSSCC18BarCodeType(),
							new StiUpcABarCodeType(),
							new StiUpcEBarCodeType(),
							new StiUpcSup2BarCodeType(),
							new StiUpcSup5BarCodeType(),
							new StiInterleaved2of5BarCodeType(),
							new StiStandard2of5BarCodeType()
						};
						barCodes = (List<StiBarCodeTypeService>)obj;
					}
					return (List<StiBarCodeTypeService>)obj;
				}
			}
		}

		public static List<StiArea> ChartAreas
		{
			get
			{
				lock (lockObject)
				{
					return chartAreas ?? (chartAreas = new List<StiArea>
					{
						new StiClusteredColumnArea(),
						new StiHistogramArea(),
						new StiLineArea(),
						new StiAreaArea(),
						new StiSplineArea(),
						new StiSplineAreaArea(),
						new StiStackedColumnArea(),
						new StiStackedLineArea(),
						new StiStackedAreaArea(),
						new StiStackedSplineArea(),
						new StiStackedSplineAreaArea(),
						new StiSteppedLineArea(),
						new StiSteppedAreaArea(),
						new StiFullStackedColumnArea(),
						new StiFullStackedLineArea(),
						new StiFullStackedAreaArea(),
						new StiFullStackedSplineArea(),
						new StiFullStackedSplineAreaArea(),
						new StiClusteredBarArea(),
						new StiStackedBarArea(),
						new StiFullStackedBarArea(),
						new StiPieArea(),
						new StiPie3dArea(),
						new StiDoughnutArea(),
						new StiRadarPointArea(),
						new StiRadarLineArea(),
						new StiRadarAreaArea(),
						new StiGanttArea(),
						new StiScatterArea(),
						new StiBubbleArea(),
						new StiRangeArea(),
						new StiSplineRangeArea(),
						new StiSteppedRangeArea(),
						new StiRangeBarArea(),
						new StiCandlestickArea(),
						new StiStockArea(),
						new StiFunnelArea(),
						new StiFunnelWeightedSlicesArea(),
						new StiTreemapArea(),
						new StiPictorialArea(),
						new StiParetoArea(),
						new StiWaterfallArea(),
						new StiSunburstArea(),
						new StiBoxAndWhiskerArea()
					}.OrderBy((StiArea a) => a.Core.Position).ToList());
				}
			}
		}

		public static List<StiSeriesLabels> ChartSerieLabels
		{
			get
			{
				lock (lockObject)
				{
					return chartSerieLabels ?? (chartSerieLabels = new List<StiSeriesLabels>
					{
						new StiNoneLabels(),
						new StiCenterAxisLabels(),
						new StiOutsideAxisLabels(),
						new StiInsideBaseAxisLabels(),
						new StiInsideEndAxisLabels(),
						new StiOutsideBaseAxisLabels(),
						new StiOutsideEndAxisLabels(),
						new StiLeftAxisLabels(),
						new StiValueAxisLabels(),
						new StiRightAxisLabels(),
						new StiInsideEndPieLabels(),
						new StiCenterPieLabels(),
						new StiOutsidePieLabels(),
						new StiTwoColumnsPieLabels(),
						new StiCenterPie3dLabels(),
						new StiCenterFunnelLabels(),
						new StiOutsideRightFunnelLabels(),
						new StiOutsideLeftFunnelLabels(),
						new StiCenterTreemapLabels()
					}.OrderBy((StiSeriesLabels a) => a.Core.Position).ToList());
				}
			}
		}

		public static List<StiSeries> ChartSeries
		{
			get
			{
				lock (lockObject)
				{
					object obj = chartSeries;
					if (obj == null)
					{
						obj = new List<StiSeries>
						{
							new StiClusteredColumnSeries(),
							new StiHistogramSeries(),
							new StiLineSeries(),
							new StiAreaSeries(),
							new StiSplineSeries(),
							new StiSplineAreaSeries(),
							new StiStackedColumnSeries(),
							new StiStackedLineSeries(),
							new StiStackedAreaSeries(),
							new StiStackedSplineSeries(),
							new StiStackedSplineAreaSeries(),
							new StiSteppedLineSeries(),
							new StiSteppedAreaSeries(),
							new StiFullStackedColumnSeries(),
							new StiFullStackedLineSeries(),
							new StiFullStackedAreaSeries(),
							new StiFullStackedSplineSeries(),
							new StiFullStackedSplineAreaSeries(),
							new StiClusteredBarSeries(),
							new StiStackedBarSeries(),
							new StiFullStackedBarSeries(),
							new StiDoughnutSeries(),
							new StiGanttSeries(),
							new StiPieSeries(),
							new StiPie3dSeries(),
							new StiBubbleSeries(),
							new StiScatterSeries(),
							new StiScatterLineSeries(),
							new StiScatterSplineSeries(),
							new StiRadarPointSeries(),
							new StiRadarLineSeries(),
							new StiRadarAreaSeries(),
							new StiFunnelSeries(),
							new StiStockSeries(),
							new StiCandlestickSeries(),
							new StiFunnelWeightedSlicesSeries(),
							new StiSteppedRangeSeries(),
							new StiSplineRangeSeries(),
							new StiRangeSeries(),
							new StiRangeBarSeries(),
							new StiTreemapSeries(),
							new StiPictorialSeries(),
							new StiParetoSeries(),
							new StiWaterfallSeries(),
							new StiSunburstSeries(),
							new StiBoxAndWhiskerSeries()
						};
						chartSeries = (List<StiSeries>)obj;
					}
					return (List<StiSeries>)obj;
				}
			}
		}

		public static List<Stimulsoft.Report.Chart.StiChartStyle> ChartStyles
		{
			get
			{
				lock (lockObject)
				{
					return chartStyles ?? (chartStyles = new List<Stimulsoft.Report.Chart.StiChartStyle>
					{
						new StiStyle01(),
						new StiStyle02(),
						new StiStyle03(),
						new StiStyle04(),
						new StiStyle05(),
						new StiStyle06(),
						new StiStyle07(),
						new StiStyle08(),
						new StiStyle09(),
						new StiStyle10(),
						new StiStyle11(),
						new StiStyle12(),
						new StiStyle13(),
						new StiStyle14(),
						new StiStyle15(),
						new StiStyle16(),
						new StiStyle17(),
						new StiStyle18(),
						new StiStyle19(),
						new StiStyle20(),
						new StiStyle21(),
						new StiStyle22(),
						new StiStyle23(),
						new StiStyle24(),
						new StiStyle25(),
						new StiStyle26(),
						new StiStyle27(),
						new StiStyle28(),
						new StiStyle29(),
						new StiStyle30(),
						new StiStyle31(),
						new StiStyle32(),
						new StiStyle33(),
						new StiStyle34(),
						new StiStyle35()
					}.OrderBy((Stimulsoft.Report.Chart.StiChartStyle s) => ((object)s).ToString()).ToList());
				}
			}
		}

		public static List<StiTrendLine> ChartTrendLines
		{
			get
			{
				lock (lockObject)
				{
					object obj = chartTrendLines;
					if (obj == null)
					{
						obj = new List<StiTrendLine>
						{
							new StiTrendLineNone(),
							new StiTrendLineLinear(),
							new StiTrendLineExponential(),
							new StiTrendLineLogarithmic()
						};
						chartTrendLines = (List<StiTrendLine>)obj;
					}
					return (List<StiTrendLine>)obj;
				}
			}
		}

		public static List<StiComponent> Components
		{
			get
			{
				lock (lockObject)
				{
					if (components == null)
					{
						components = new List<StiComponent>
						{
							new StiReportTitleBand(),
							new StiReportSummaryBand(),
							new StiPageHeaderBand(),
							new StiPageFooterBand(),
							new StiGroupHeaderBand(),
							new StiGroupFooterBand(),
							new StiHeaderBand(),
							new StiFooterBand(),
							new StiColumnHeaderBand(),
							new StiColumnFooterBand(),
							new StiHierarchicalBand(),
							new StiEmptyBand(),
							new StiOverlayBand(),
							new StiDataBand(),
							new StiTable(),
							new StiTableCell(),
							new StiTableCellCheckBox(),
							new StiTableCellImage(),
							new StiTableCellRichText(),
							new StiPage(),
							new Stimulsoft.Report.Components.StiPanel(),
							new StiContainer(),
							new StiCheckBox(),
							new StiChildBand(),
							new StiClone(),
							new StiText(),
							new StiSystemText(),
							new StiTextInCells(),
							new StiImage(),
							new StiRichText(),
							new StiShape(),
							new StiSubReport(),
							new StiWinControl(),
							new StiZipCode(),
							new StiSparkline(),
							new StiTableOfContents(),
							new StiHorizontalLinePrimitive(),
							new StiVerticalLinePrimitive(),
							new StiRectanglePrimitive(),
							new StiRoundedRectanglePrimitive(),
							new StiStartPointPrimitive(),
							new StiEndPointPrimitive(),
							new StiCrossDataBand(),
							new StiCrossFooterBand(),
							new StiCrossGroupFooterBand(),
							new StiCrossGroupHeaderBand(),
							new StiCrossHeaderBand(),
							new StiBarCode(),
							new StiCrossTab(),
							new StiCrossColumn(),
							new StiCrossTotal(),
							new StiCrossRowTotal(),
							new StiCrossColumnTotal(),
							new StiCrossRow(),
							new StiCrossSummary(),
							new StiCrossTitle(),
							new StiCrossSummaryHeader(),
							new StiChart(),
							new StiButtonControl(),
							new StiCheckBoxControl(),
							new StiCheckedListBoxControl(),
							new StiComboBoxControl(),
							new StiDateTimePickerControl(),
							new StiForm(),
							new StiGridControl(),
							new StiGroupBoxControl(),
							new StiLabelControl(),
							new StiListBoxControl(),
							new StiLookUpBoxControl(),
							new StiNumericUpDownControl(),
							new StiPanelControl(),
							new StiPictureBoxControl(),
							new StiRadioButtonControl(),
							new StiTextBoxControl(),
							new StiRichTextBoxControl(),
							new StiTreeViewControl(),
							new StiListViewControl(),
							new StiGauge(),
							new StiMap(),
							new StiMathFormula()
						};
						List<StiComponent> list = StiDashboardAssembly.LoadDashboardElements();
						if (list != null)
						{
							components.AddRange(list);
						}
						List<StiComponent> list2 = StiAppAssembly.LoadAppComponents();
						if (list2 != null)
						{
							components.AddRange(list2);
						}
						components = components.OrderBy((StiComponent f) => f.ToolboxPosition).ToList();
					}
					return components;
				}
			}
		}

		public static List<StiCustomValueBase> CustomValues
		{
			get
			{
				lock (lockObject)
				{
					object obj = customValues;
					if (obj == null)
					{
						obj = new List<StiCustomValueBase>
						{
							new StiLinearTickLabelCustomValue(),
							new StiLinearTickMarkCustomValue(),
							new StiRadialTickLabelCustomValue(),
							new StiRadialTickMarkCustomValue()
						};
						customValues = (List<StiCustomValueBase>)obj;
					}
					return (List<StiCustomValueBase>)obj;
				}
			}
		}

		public static List<StiDataAdapterService> DataAdapters
		{
			get
			{
				lock (lockObject)
				{
					object obj = dataAdapters;
					if (obj == null)
					{
						obj = new List<StiDataAdapterService>
						{
							new StiBusinessObjectAdapterService(),
							new StiCrossTabAdapterService(),
							new StiDataTableAdapterService(),
							new StiDataViewAdapterService(),
							new StiVirtualAdapterService(),
							new StiUserAdapterService(),
							new StiCsvAdapterService(),
							new StiDBaseAdapterService(),
							new StiDB2AdapterService(),
							new StiDotConnectUniversalAdapterService(),
							new StiFirebirdAdapterService(),
							new StiInformixAdapterService(),
							new StiMySqlAdapterService(),
							new StiMSAccessAdapterService(),
							new StiOdbcAdapterService(),
							new StiOleDbAdapterService(),
							new StiOracleAdapterService(),
							new StiPostgreSQLAdapterService(),
							new StiSQLiteAdapterService(),
							new StiSqlAdapterService(),
							new StiSqlCeAdapterService(),
							new StiSybaseAdapterService(),
							new StiSybaseAdsAdapterService(),
							new StiTeradataAdapterService(),
							new StiVistaDBAdapterService(),
							new StiODataAdapterService(),
							new StiMongoDbAdapterService(),
							new StiGoogleSheetsAdapterService(),
							new StiAzureTableStorageAdapterService(),
							new StiAzureBlobStorageAdapterService(),
							new StiCosmosDbAdapterService(),
							new StiDataWorldAdapterService(),
							new StiQuickBooksAdapterService(),
							new StiFirebaseAdapterService(),
							new StiBigQueryAdapterService(),
							new StiGoogleAnalyticsAdapterService()
						};
						dataAdapters = (List<StiDataAdapterService>)obj;
					}
					return (List<StiDataAdapterService>)obj;
				}
			}
		}

		public static List<StiDatabase> Databases
		{
			get
			{
				lock (lockObject)
				{
					object obj = databases;
					if (obj == null)
					{
						obj = new List<StiDatabase>
						{
							new StiDB2Database(),
							new StiDotConnectUniversalDatabase(),
							new StiFirebirdDatabase(),
							new StiInformixDatabase(),
							new StiMySqlDatabase(),
							new StiMSAccessDatabase(),
							new StiOdbcDatabase(),
							new StiOleDbDatabase(),
							new StiOracleDatabase(),
							new StiPostgreSQLDatabase(),
							new StiSQLiteDatabase(),
							new StiSqlDatabase(),
							new StiSqlCeDatabase(),
							new StiSybaseDatabase(),
							new StiSybaseAdsDatabase(),
							new StiTeradataDatabase(),
							new StiVistaDBDatabase(),
							new StiODataDatabase(),
							new StiAzureTableStorageDatabase(),
							new StiCosmosDbDatabase(),
							new StiMongoDbDatabase(),
							new StiGoogleSheetsDatabase(),
							new StiCsvDatabase(),
							new StiDBaseDatabase(),
							new StiExcelDatabase(),
							new StiJsonDatabase(),
							new StiXmlDatabase(),
							new StiGisDatabase(),
							new StiDataWorldDatabase(),
							new StiQuickBooksDatabase(),
							new StiFirebaseDatabase(),
							new StiBigQueryDatabase(),
							new StiGoogleAnalyticsDatabase(),
							new StiAzureSqlDatabase(),
							new StiAzureBlobStorageDatabase()
						};
						databases = (List<StiDatabase>)obj;
					}
					return (List<StiDatabase>)obj;
				}
			}
		}

		public static List<StiDataSource> DataSource
		{
			get
			{
				lock (lockObject)
				{
					object obj = dataSource;
					if (obj == null)
					{
						obj = new List<StiDataSource>
						{
							new StiDataWorldSource(),
							new StiDataTableSource(),
							new StiDataViewSource(),
							new StiUndefinedDataSource(),
							new StiCsvSource(),
							new StiDBaseSource(),
							new StiBusinessObjectSource(),
							new StiCrossTabDataSource(),
							new StiEnumerableSource(),
							new StiUserSource(),
							new StiVirtualSource(),
							new StiOracleODPSource(),
							new StiDB2Source(),
							new StiFirebirdSource(),
							new StiInformixSource(),
							new StiMSAccessSource(),
							new StiMySqlSource(),
							new StiOdbcSource(),
							new StiOleDbSource(),
							new StiOracleSource(),
							new StiPostgreSQLSource(),
							new StiSqlCeSource(),
							new StiSQLiteSource(),
							new StiSqlSource(),
							new StiSybaseSource(),
							new StiTeradataSource(),
							new StiVistaDBSource(),
							new StiODataSource(),
							new StiQuickBooksSource(),
							new StiDataTransformation(),
							new StiBigQuerySource(),
							new StiAzureBlobStorageSource()
						};
						dataSource = (List<StiDataSource>)obj;
					}
					return (List<StiDataSource>)obj;
				}
			}
		}

		public static List<StiDataTableSetNameService> DataTableSetName
		{
			get
			{
				lock (lockObject)
				{
					object obj = dataTableSetName;
					if (obj == null)
					{
						obj = new List<StiDataTableSetNameService>
						{
							new StiDataTableSetNameService()
						};
						dataTableSetName = (List<StiDataTableSetNameService>)obj;
					}
					return (List<StiDataTableSetNameService>)obj;
				}
			}
		}

		public static List<StiDialogsProvider> DialogProviders
		{
			get
			{
				lock (lockObject)
				{
					object obj = dialogProviders;
					if (obj == null)
					{
						obj = new List<StiDialogsProvider>
						{
							new StiWinDialogsProvider()
						};
						dialogProviders = (List<StiDialogsProvider>)obj;
					}
					return (List<StiDialogsProvider>)obj;
				}
			}
		}

		public static List<StiDictionarySLService> DictionarySLs
		{
			get
			{
				lock (lockObject)
				{
					object obj = dictionarySLs;
					if (obj == null)
					{
						obj = new List<StiDictionarySLService>
						{
							new StiXmlDictionarySLService()
						};
						dictionarySLs = (List<StiDictionarySLService>)obj;
					}
					return (List<StiDictionarySLService>)obj;
				}
			}
		}

		public static List<StiDocumentSLService> DocumentSLs
		{
			get
			{
				lock (lockObject)
				{
					object obj = documentSLs;
					if (obj == null)
					{
						obj = new List<StiDocumentSLService>
						{
							new StiXmlDocumentSLService(),
							new StiPackedDocumentSLService(),
							new StiEncryptedDocumentSLService(),
							new StiJsonDocumentSLService()
						};
						documentSLs = (List<StiDocumentSLService>)obj;
					}
					return (List<StiDocumentSLService>)obj;
				}
			}
		}

		public static List<StiDotMatrixViewerConfigService> DotMatrixViewerConfig
		{
			get
			{
				lock (lockObject)
				{
					object obj = dotMatrixViewerConfig;
					if (obj == null)
					{
						obj = new List<StiDotMatrixViewerConfigService>
						{
							new StiDotMatrixViewerConfigService()
						};
						dotMatrixViewerConfig = (List<StiDotMatrixViewerConfigService>)obj;
					}
					return (List<StiDotMatrixViewerConfigService>)obj;
				}
			}
		}

		public static List<StiExportService> Exports
		{
			get
			{
				lock (lockObject)
				{
					object obj = exports;
					if (obj == null)
					{
						obj = new List<StiExportService>
						{
							new StiPdfExportService(),
							new StiXpsExportService(),
							new StiPpt2007ExportService(),
							new StiHtmlExportService(),
							new StiHtml5ExportService(),
							new StiMhtExportService(),
							new StiExcelXmlExportService(),
							new StiExcelExportService(),
							new StiExcel2007ExportService(),
							new StiOdsExportService(),
							new StiTxtExportService(),
							new StiRtfExportService(),
							new StiWord2007ExportService(),
							new StiOdtExportService(),
							new StiDataExportService(),
							new StiImageExportService()
						};
						exports = (List<StiExportService>)obj;
					}
					return (List<StiExportService>)obj;
				}
			}
		}

		public static List<StiFormatService> Formats
		{
			get
			{
				lock (lockObject)
				{
					return formats ?? (formats = new List<StiFormatService>
					{
						new StiBooleanFormatService(),
						new StiNumberFormatService(),
						new StiCurrencyFormatService(),
						new StiCustomFormatService(),
						new StiDateFormatService(),
						new StiGeneralFormatService(),
						new StiPercentageFormatService(),
						new StiTimeFormatService()
					}.OrderBy((StiFormatService f) => f.Position).ToList());
				}
			}
		}

		public static List<StiGaugeElement> GaugeElements
		{
			get
			{
				lock (lockObject)
				{
					object obj = gaugeElements;
					if (obj == null)
					{
						obj = new List<StiGaugeElement>
						{
							new StiLinearBar(),
							new StiLinearMarker(),
							new StiNeedle(),
							new StiRadialBar(),
							new StiRadialMarker(),
							new StiStateIndicator(),
							new StiLinearRangeList(),
							new StiRadialRangeList(),
							new StiLinearTickLabelCustom(),
							new StiLinearTickLabelMajor(),
							new StiLinearTickLabelMinor(),
							new StiLinearTickMarkCustom(),
							new StiLinearTickMarkMajor(),
							new StiLinearTickMarkMinor(),
							new StiRadialTickLabelCustom(),
							new StiRadialTickLabelMajor(),
							new StiRadialTickLabelMinor(),
							new StiRadialTickMarkCustom(),
							new StiRadialTickMarkMajor(),
							new StiRadialTickMarkMinor()
						};
						gaugeElements = (List<StiGaugeElement>)obj;
					}
					return (List<StiGaugeElement>)obj;
				}
			}
		}

		public static List<StiScaleBase> GaugeScales
		{
			get
			{
				lock (lockObject)
				{
					object obj = gaugeScales;
					if (obj == null)
					{
						obj = new List<StiScaleBase>
						{
							new StiLinearScale(),
							new StiRadialScale()
						};
						gaugeScales = (List<StiScaleBase>)obj;
					}
					return (List<StiScaleBase>)obj;
				}
			}
		}

		public static List<StiGaugeStyleXF> GaugeStyles
		{
			get
			{
				lock (lockObject)
				{
					return gaugeStyles ?? (gaugeStyles = new List<StiGaugeStyleXF>
					{
						new StiGaugeStyleXF29(),
						new StiGaugeStyleXF24(),
						new StiGaugeStyleXF27(),
						new StiGaugeStyleXF26(),
						new StiGaugeStyleXF25(),
						new StiGaugeStyleXF28(),
						new StiGaugeStyleXF30(),
						new StiGaugeStyleXF31(),
						new StiGaugeStyleXF32(),
						new StiGaugeStyleXF33(),
						new StiGaugeStyleXF34(),
						new StiGaugeStyleXF35()
					}.OrderBy((StiGaugeStyleXF s) => ((object)s).ToString()).ToList());
				}
			}
		}

		public static List<StiIndicatorRangeInfo> IndicatorRanges
		{
			get
			{
				lock (lockObject)
				{
					object obj = indicatorRanges;
					if (obj == null)
					{
						obj = new List<StiIndicatorRangeInfo>
						{
							new StiLinearIndicatorRangeInfo(),
							new StiRadialIndicatorRangeInfo()
						};
						indicatorRanges = (List<StiIndicatorRangeInfo>)obj;
					}
					return (List<StiIndicatorRangeInfo>)obj;
				}
			}
		}

		public static List<StiLogService> Logs
		{
			get
			{
				lock (lockObject)
				{
					object obj = logs;
					if (obj == null)
					{
						obj = new List<StiLogService>
						{
							new StiLogService()
						};
						logs = (List<StiLogService>)obj;
					}
					return (List<StiLogService>)obj;
				}
			}
		}

		public static List<StiMapStyleFX> MapStyles
		{
			get
			{
				lock (lockObject)
				{
					object obj = mapStyles;
					if (obj == null)
					{
						obj = new List<StiMapStyleFX>
						{
							new StiMap21StyleFX(),
							new StiMap24StyleFX(),
							new StiMap25StyleFX(),
							new StiMap26StyleFX(),
							new StiMap27StyleFX(),
							new StiMap28StyleFX(),
							new StiMap29StyleFX(),
							new StiMap30StyleFX(),
							new StiMap31StyleFX(),
							new StiMap32StyleFX(),
							new StiMap33StyleFX(),
							new StiMap34StyleFX(),
							new StiMap35StyleFX()
						};
						mapStyles = (List<StiMapStyleFX>)obj;
					}
					return (List<StiMapStyleFX>)obj;
				}
			}
		}

		public static List<StiPageSLService> PageSLs
		{
			get
			{
				lock (lockObject)
				{
					object obj = pageSLs;
					if (obj == null)
					{
						obj = new List<StiPageSLService>
						{
							new StiXmlPageSLService()
						};
						pageSLs = (List<StiPageSLService>)obj;
					}
					return (List<StiPageSLService>)obj;
				}
			}
		}

		public static List<StiRangeBase> Ranges
		{
			get
			{
				lock (lockObject)
				{
					object obj = ranges;
					if (obj == null)
					{
						obj = new List<StiRangeBase>
						{
							new StiLinearRange(),
							new StiRadialRange()
						};
						ranges = (List<StiRangeBase>)obj;
					}
					return (List<StiRangeBase>)obj;
				}
			}
		}

		public static List<StiReportSLService> ReportSLs
		{
			get
			{
				lock (lockObject)
				{
					if (Designer.PlaceMRZBeforeMRTInSaveDialog)
					{
						object obj = reportSLs;
						if (obj == null)
						{
							obj = new List<StiReportSLService>
							{
								new StiJsonReportSLService(),
								new StiPackedReportSLService(),
								new StiEncryptedReportSLService(),
								new StiXmlReportSLService()
							};
							reportSLs = (List<StiReportSLService>)obj;
						}
						return (List<StiReportSLService>)obj;
					}
					object obj2 = reportSLs;
					if (obj2 == null)
					{
						obj2 = new List<StiReportSLService>
						{
							new StiXmlReportSLService(),
							new StiEncryptedReportSLService(),
							new StiPackedReportSLService(),
							new StiJsonReportSLService()
						};
						reportSLs = (List<StiReportSLService>)obj2;
					}
					return (List<StiReportSLService>)obj2;
				}
			}
		}

		public static List<StiShapeTypeService> Shapes
		{
			get
			{
				lock (lockObject)
				{
					object obj = shapes;
					if (obj == null)
					{
						obj = new List<StiShapeTypeService>
						{
							new StiArrowShapeType(),
							new StiDiagonalDownLineShapeType(),
							new StiDiagonalUpLineShapeType(),
							new StiHorizontalLineShapeType(),
							new StiLeftAndRightLineShapeType(),
							new StiOvalShapeType(),
							new StiRectangleShapeType(),
							new StiRoundedRectangleShapeType(),
							new StiOctagonShapeType(),
							new StiTopAndBottomLineShapeType(),
							new StiTriangleShapeType(),
							new StiVerticalLineShapeType(),
							new StiComplexArrowShapeType(),
							new StiBentArrowShapeType(),
							new StiChevronShapeType(),
							new StiDivisionShapeType(),
							new StiEqualShapeType(),
							new StiFlowchartCardShapeType(),
							new StiFlowchartCollateShapeType(),
							new StiFlowchartDecisionShapeType(),
							new StiFlowchartManualInputShapeType(),
							new StiFlowchartOffPageConnectorShapeType(),
							new StiFlowchartPreparationShapeType(),
							new StiFlowchartSortShapeType(),
							new StiFrameShapeType(),
							new StiMinusShapeType(),
							new StiMultiplyShapeType(),
							new StiParallelogramShapeType(),
							new StiPlusShapeType(),
							new StiRegularPentagonShapeType(),
							new StiTrapezoidShapeType(),
							new StiSnipSameSideCornerRectangleShapeType(),
							new StiSnipDiagonalSideCornerRectangleShapeType()
						};
						shapes = (List<StiShapeTypeService>)obj;
					}
					return (List<StiShapeTypeService>)obj;
				}
			}
		}

		public static List<StiTableStyleFX> TableStyles
		{
			get
			{
				lock (lockObject)
				{
					object obj = tableStyles;
					if (obj == null)
					{
						obj = new List<StiTableStyleFX>
						{
							new StiTable21StyleFX(),
							new StiTable24StyleFX(),
							new StiTable25StyleFX(),
							new StiTable26StyleFX(),
							new StiTable27StyleFX()
						};
						tableStyles = (List<StiTableStyleFX>)obj;
					}
					return (List<StiTableStyleFX>)obj;
				}
			}
		}

		public static List<StiViewerConfigService> ViewerConfig
		{
			get
			{
				lock (lockObject)
				{
					object obj = viewerConfig;
					if (obj == null)
					{
						obj = new List<StiViewerConfigService>
						{
							new StiViewerConfigService()
						};
						viewerConfig = (List<StiViewerConfigService>)obj;
					}
					return (List<StiViewerConfigService>)obj;
				}
			}
		}
	}

	public class Viewer
	{
		public class DotMatrix
		{
			[StiSerializable]
			[DefaultValue(false)]
			public static bool GroupBoxBorderType
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "groupBoxBorderType");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "groupBoxBorderType", value);
					StiSettings.Save();
				}
			}

			[Description("Gets or sets a value which controls collapsing of the Group Box Encoding of the Dot-Matrix window of the report. ")]
			[StiSerializable]
			[DefaultValue(false)]
			public static bool GroupBoxEncoding
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "groupBoxEncoding");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "groupBoxEncoding", value);
					StiSettings.Save();
				}
			}

			[Description("Gets or sets a value which controls collapsing of the Group Box Rrefresh of the Dot-Matrix window of the report.")]
			[StiSerializable]
			[DefaultValue(false)]
			public static bool GroupBoxRefresh
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "groupBoxRefresh");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "groupBoxRefresh", value);
					StiSettings.Save();
				}
			}

			[Description("Gets or sets a value which contols a collapsing of the Group Box Settings of the Dot-Matrix window of the report.")]
			[StiSerializable]
			[DefaultValue(false)]
			public static bool GroupBoxSettings
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "groupBoxSettings");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "groupBoxSettings", value);
					StiSettings.Save();
				}
			}

			[Description("Gets or sets a value which controls a collapsing of the Group Box Zoom.")]
			[StiSerializable]
			[DefaultValue(false)]
			public static bool GroupBoxZoom
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "groupBoxZoom");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "groupBoxZoom", value);
					StiSettings.Save();
				}
			}

			[Description("Gets or sets a value which controls a visibillity of the Dot-Matrix Mode button of the report.")]
			[StiSerializable]
			[DefaultValue(false)]
			public static bool DotMatrixMode
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "DotMatrixMode");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "DotMatrixMode", value);
					StiSettings.Save();
				}
			}

			[StiSerializable]
			[DefaultValue(false)]
			public static bool CutLongLines
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "CutLongLines");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "CutLongLines", value);
					StiSettings.Save();
				}
			}

			[StiSerializable]
			[DefaultValue(false)]
			public static bool DrawBorder
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "DrawBorder");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "DrawBorder", value);
					StiSettings.Save();
				}
			}

			[StiSerializable]
			[DefaultValue(false)]
			public static bool KillSpaceLines
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "KillSpaceLines");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "KillSpaceLines", value);
					StiSettings.Save();
				}
			}

			[StiSerializable]
			[DefaultValue(false)]
			public static bool KillSpaceGraphLines
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "KillSpaceGraphLines");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "KillSpaceGraphLines", value);
					StiSettings.Save();
				}
			}

			[StiSerializable]
			[DefaultValue(false)]
			public static bool PutFeedPageCode
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "PutFeedPageCode");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "PutFeedPageCode", value);
					StiSettings.Save();
				}
			}

			[StiSerializable]
			[DefaultValue(false)]
			public static bool Simple
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "Simple");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "Simple", value);
					StiSettings.Save();
				}
			}

			[StiSerializable]
			[DefaultValue(false)]
			public static bool UnicodeSingle
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "UnicodeSingle");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "UnicodeSingle", value);
					StiSettings.Save();
				}
			}

			[StiSerializable]
			[DefaultValue(false)]
			public static bool UnicodeDouble
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "UnicodeDouble");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "UnicodeDouble", value);
					StiSettings.Save();
				}
			}

			[StiSerializable]
			[DefaultValue(false)]
			public static bool AutoRefresh
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "AutoRefresh");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "AutoRefresh", value);
					StiSettings.Save();
				}
			}

			[StiSerializable]
			[DefaultValue("")]
			public static string ZoomX
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetStr("Viewer", "ZoomX");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "ZoomX", value);
					StiSettings.Save();
				}
			}

			[StiSerializable]
			[DefaultValue("")]
			public static string ZoomY
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetStr("Viewer", "ZoomY");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "ZoomY", value);
					StiSettings.Save();
				}
			}

			[StiSerializable]
			[DefaultValue(-1)]
			public static int Encoding
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetInt("Viewer", "Encoding");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "Encoding", value);
					StiSettings.Save();
				}
			}

			[StiSerializable]
			[DefaultValue(false)]
			public static bool UseEscapeCodes
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "UseEscapeCodes");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "UseEscapeCodes", value);
					StiSettings.Save();
				}
			}

			[StiSerializable]
			[DefaultValue("")]
			public static string EscapeCodesCollectionName
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetStr("Viewer", "EscapeCodesCollectionName");
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "EscapeCodesCollectionName", value);
					StiSettings.Save();
				}
			}

			[DefaultValue(true)]
			[Description("Class for adjusting a Dot-Matrix window of the report.")]
			[StiSerializable]
			public static bool ShowInTaskbar { get; set; } = true;


			[DefaultValue(/*Could not decode attribute arguments.*/)]
			[Description("Gets or sets window state of the viewer window.")]
			[StiSerializable]
			public static FormWindowState ViewerWindowState { get; set; } = (FormWindowState)2;


			public static Encoding[] DefaultEncodings { get; set; }

			public static Icon ViewerIcon { get; set; }

			public static object ViewerWpfIcon { get; set; }
		}

		public static class HotKeys
		{
			[Description("Get or set value indicating whether the Escape shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool EscapeEnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Ctrl + P shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool CtrlPEnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Ctrl + O shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool CtrlOEnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Ctrl + S shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool CtrlSEnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Ctrl + G shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool CtrlGEnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Ctrl + Enter shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool CtrlEnterEnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Ctrl + R shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool CtrlREnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Ctrl + Shift + N shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool CtrlShiftNEnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Ctrl + Shift + P shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool CtrlShiftPEnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Ctrl + Shift + D shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool CtrlShiftDEnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Ctrl + Shift + E shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool CtrlShiftEEnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Ctrl + Shift + S shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool CtrlShiftSEnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Ctrl + B shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool CtrlBEnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Ctrl + T shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool CtrlTEnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Ctrl + F shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool CtrlFEnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Ctrl + E shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool CtrlEEnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Ctrl + D shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool CtrlDEnabled { get; set; } = true;


			[Description("Get or set value indicating whether the Shift + F2 shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShiftF2Enabled { get; set; } = true;


			[Description("Get or set value indicating whether the Shift + F3 shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShiftF3Enabled { get; set; } = true;


			[Description("Get or set value indicating whether the Shift + F4 shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShiftF4Enabled { get; set; } = true;


			[Description("Get or set value indicating whether the F2 shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool F2Enabled { get; set; } = true;


			[Description("Get or set value indicating whether the F3 shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool F3Enabled { get; set; } = true;


			[Description("Get or set value indicating whether the F4 shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool F4Enabled { get; set; } = true;


			[Description("Get or set value indicating whether the F5 shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool F5Enabled { get; set; } = true;


			[Description("Get or set value indicating whether the F7 shortcut is enabled.")]
			[DefaultValue(true)]
			[StiSerializable]
			public static bool F7Enabled { get; set; } = true;


			public static void DisableAllHotkeys()
			{
				SwitchAllHotkeys(enabled: false);
			}

			public static void EnableAllHotkeys()
			{
				SwitchAllHotkeys(enabled: true);
			}

			public static void SwitchAllHotkeys(bool enabled)
			{
				CtrlBEnabled = enabled;
				CtrlEEnabled = enabled;
				CtrlDEnabled = enabled;
				CtrlFEnabled = enabled;
				CtrlOEnabled = enabled;
				CtrlPEnabled = enabled;
				CtrlSEnabled = enabled;
				CtrlGEnabled = enabled;
				CtrlREnabled = enabled;
				CtrlTEnabled = enabled;
				CtrlEnterEnabled = enabled;
				CtrlShiftDEnabled = enabled;
				CtrlShiftEEnabled = enabled;
				CtrlShiftNEnabled = enabled;
				CtrlShiftPEnabled = enabled;
				CtrlShiftSEnabled = enabled;
				EscapeEnabled = enabled;
				F2Enabled = enabled;
				F3Enabled = enabled;
				F4Enabled = enabled;
				F5Enabled = enabled;
				F7Enabled = enabled;
				ShiftF2Enabled = enabled;
				ShiftF3Enabled = enabled;
				ShiftF4Enabled = enabled;
			}
		}

		public class Map
		{
			[StiSerializable]
			[DefaultValue(true)]
			public static bool ShowZoomPanel { get; set; } = true;

		}

		public class Pins
		{
			[DefaultValue(false)]
			[Description("Gets or sets value which indicates orientation of quick buttons pin.")]
			[StiSerializable]
			public static bool QuickButtonsRightToLeft { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets value which indicates orientation of events pin.")]
			[StiSerializable]
			public static bool EventsRightToLeft { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets value which indicates orientation of conditions pin.")]
			[StiSerializable]
			public static bool ConditionsRightToLeft { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets value which indicates orientation of filters pin.")]
			[StiSerializable]
			public static bool FiltersRightToLeft { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets value which indicates orientation of interaction sort pin.")]
			[StiSerializable]
			public static bool InteractionSortRightToLeft { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets value which indicates orientation of interaction collapsing pin.")]
			[StiSerializable]
			public static bool InteractionCollapsingRightToLeft { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets value which indicates orientation of inheritance pin.")]
			[StiSerializable]
			public static bool InheritedRightToLeft { get; set; }

			[DefaultValue(false)]
			[Description("Gets or sets value which indicates orientation of order and quick info pin.")]
			[StiSerializable]
			public static bool OrderAndQuickInfoRightToLeft { get; set; }
		}

		public class Windows
		{
			public sealed class Exports
			{
				public static class States
				{
					public static bool IsDocumentVisible
					{
						get
						{
							if (!ShowPdf && !ShowXps)
							{
								return ShowPpt2007;
							}
							return true;
						}
					}

					public static bool IsWebVisible
					{
						get
						{
							if (!ShowHtml)
							{
								return ShowMht;
							}
							return true;
						}
					}

					public static bool IsTextVisible
					{
						get
						{
							if (!ShowText && !ShowRtf && !ShowWord2007)
							{
								return ShowOdt;
							}
							return true;
						}
					}

					public static bool IsCalcVisible
					{
						get
						{
							if (!ShowExcel && !ShowExcelXml && !ShowExcel2007)
							{
								return ShowOds;
							}
							return true;
						}
					}

					public static bool IsDataVisible
					{
						get
						{
							if (!ShowCsv && !ShowDbf && !ShowXml && !ShowDif)
							{
								return ShowSylk;
							}
							return true;
						}
					}

					public static bool IsImagesVisible
					{
						get
						{
							if (!IsBitmapImagesVisible)
							{
								return IsVectorImagesVisible;
							}
							return true;
						}
					}

					public static bool IsBitmapImagesVisible
					{
						get
						{
							if (!ShowBmp && !ShowGif && !ShowJpeg && !ShowPcx && !ShowPng)
							{
								return ShowTiff;
							}
							return true;
						}
					}

					public static bool IsVectorImagesVisible
					{
						get
						{
							if (!ShowMetafile && !ShowSvg)
							{
								return ShowSvgz;
							}
							return true;
						}
					}
				}

				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPdf { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowXps { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPpt2007 { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowHtml { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowHtml5 { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowMht { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowText { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowRtf { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowWord2007 { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowOdt { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowExcel { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowExcelXml { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowExcel2007 { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowOds { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowData
				{
					get
					{
						if (!ShowCsv && !ShowDbf && !ShowXml && !ShowDif && !ShowSylk)
						{
							return ShowJson;
						}
						return true;
					}
					set
					{
						bool num = (ShowJson = value);
						ShowSylk = num;
						ShowDif = num;
						ShowXml = num;
						ShowDbf = num;
						ShowCsv = num;
					}
				}

				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowCsv { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDbf { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowXml { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowDif { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowSylk { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowJson { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowImage
				{
					get
					{
						if (!ShowBmp && !ShowGif && !ShowJpeg && !ShowPcx && !ShowPng && !ShowSvg && !ShowSvgz && !ShowTiff)
						{
							return ShowMetafile;
						}
						return true;
					}
					set
					{
						bool num = (ShowMetafile = value);
						ShowTiff = num;
						ShowSvgz = num;
						ShowSvg = num;
						ShowPng = num;
						ShowPcx = num;
						ShowJpeg = num;
						ShowGif = num;
						ShowBmp = num;
					}
				}

				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowBmp { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowGif { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowJpeg { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPcx { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowPng { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowTiff { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowSvg { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowSvgz { get; set; } = true;


				[DefaultValue(true)]
				[StiSerializable]
				public static bool ShowMetafile { get; set; } = true;

			}

			public class WPFViewer
			{
				[DefaultValue(15.0)]
				[StiSerializable]
				public static double DistanceBetweenPagesHorz { get; set; } = 15.0;


				[DefaultValue(15.0)]
				[StiSerializable]
				public static double DistanceBetweenPagesVert { get; set; } = 15.0;


				[DefaultValue(10.0)]
				[StiSerializable]
				public static double DistanceFromLeft { get; set; } = 10.0;


				[DefaultValue(10.0)]
				[StiSerializable]
				public static double DistanceFromRight { get; set; } = 10.0;


				[DefaultValue(10.0)]
				[StiSerializable]
				public static double DistanceFromTop { get; set; } = 10.0;


				[DefaultValue(10.0)]
				[StiSerializable]
				public static double DistanceFromBottom { get; set; } = 10.0;


				[DefaultValue(1.02)]
				[StiSerializable]
				[Obsolete]
				[EditorBrowsable(EditorBrowsableState.Never)]
				public static double ScaleActivePage { get; set; } = 1.02;

			}

			[Obsolete("Please use property 'ShowInTaskbar' instead property 'ForceShowInTaskbar'.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool ForceShowInTaskbar { get; set; } = true;


			[Obsolete("Please use ShowSaveButton property instead ShowExportButton property.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static bool? ShowExportButton
			{
				get
				{
					return ShowSaveButton;
				}
				set
				{
					ShowSaveButton = value;
				}
			}

			[Obsolete("Please use property 'ViewerIcon' instead 'PreviewIcon' property.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static Icon PreviewIcon
			{
				get
				{
					return ViewerIcon;
				}
				set
				{
					ViewerIcon = value;
				}
			}

			[Obsolete("Please use property 'ViewerWindowState' instead 'PreviewWindowState' property.")]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public static FormWindowState PreviewWindowState
			{
				get
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					return ViewerWindowState;
				}
				set
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					ViewerWindowState = value;
				}
			}

			[Description("Gets or sets zoom of the drawing of the pages in viewer window.")]
			[StiSerializable]
			[DefaultValue(1.0)]
			public static double Zoom
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetDouble("Viewer", "Zoom", 1.0);
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "Zoom", value);
					StiSettings.Save();
				}
			}

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether to show a Toolbar in the viewer control.")]
			[StiSerializable]
			public static bool? ShowToolbar { get; set; }

			[Description("Gets or sets a value indicating whether to show all buttons for the Page View Mode. ")]
			[StiSerializable]
			[DefaultValue(null)]
			public static bool? ShowPageViewMode
			{
				get
				{
					if (!ShowPageViewSingleMode.HasValue && !ShowPageViewContinuousMode.HasValue && !ShowPageViewMultipleMode.HasValue)
					{
						return null;
					}
					return ShowPageViewSingleMode.GetValueOrDefault(true) && ShowPageViewContinuousMode.GetValueOrDefault(true) && ShowPageViewMultipleMode.GetValueOrDefault(true);
				}
				set
				{
					ShowPageViewSingleMode = value;
					ShowPageViewContinuousMode = value;
					ShowPageViewMultipleMode = value;
				}
			}

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether to show Single Page Mode. ")]
			[StiSerializable]
			public static bool? ShowPageViewSingleMode { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether to show Continues Page Mode.")]
			[StiSerializable]
			public static bool? ShowPageViewContinuousMode { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether to show MultiplePages Page Mode.")]
			[StiSerializable]
			public static bool? ShowPageViewMultipleMode { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether to show 'FullScreen' button.")]
			[StiSerializable]
			public static bool? ShowFullScreen { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether to show the Vertical Scroll Bar.")]
			[StiSerializable]
			public static bool? ShowVertScrollBar { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Status Bar is shown in the viewer window.")]
			[StiSerializable]
			public static bool? ShowStatusBar { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Print Button is shown.")]
			[StiSerializable]
			public static bool? ShowPrintButton { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Open button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowOpenButton { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Refreh button is visible in the viewer window.This property works only for the dashboard viewer.")]
			[StiSerializable]
			public static bool? ShowRefreshButton { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Save button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowSaveButton { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Save Document File button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowSaveDocumentFileButton { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the SendEMail Document File button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowSendEMailDocumentFileButton { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Send EMail button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowSendEMailButton { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Show New Page button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowPageNewButton { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Delete Page button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowPageDeleteButton { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Page Design button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowPageDesignButton { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Page Size button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowPageSizeButton { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether to show button for select mode of viewing pages in viewer window.")]
			[StiSerializable]
			public static bool? ShowPageViewModeButton { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Select Tool button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowSelectTool { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Hand Tool button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowHandTool { get; set; }

			[DefaultValue(true)]
			[Description("Gets or sets a value indicating whether the Editor Tool button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowEditorTool { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Find Tool button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowFindTool { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Zoom panel is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowZoom { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Zoom panel is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowPageControl { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the 'First Page' button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowFirstPage { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the 'Previous Page' button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowPreviousPage { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the 'Next Page' button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowNextPage { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the 'Last Page' button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowLastPage { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the BookMarks panel is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowBookmarksPanel { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Thumbnails panel is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowThumbsPanel { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Thumbnails button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowThumbsButton { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Parameters button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowParametersButton { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether the Resources button is visible in the viewer window.")]
			[StiSerializable]
			public static bool? ShowResourcesButton { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether it is will be show in context menu of viewer window.")]
			[StiSerializable]
			public static bool? ShowContextMenu { get; set; }

			[DefaultValue(null)]
			[Description("Gets or sets a value indicating whether it is will be show Close Button in the viewer window.")]
			[StiSerializable]
			public static bool? ShowCloseButton { get; set; }

			[DefaultValue("")]
			[Description("Gets or sets path for saving and loading a report template.")]
			[StiSerializable]
			public static string ReportSaveLoadPath { get; set; } = string.Empty;


			[DefaultValue("")]
			[Description("Gets or sets path for saving exported report.")]
			[StiSerializable]
			public static string ExportSaveLoadPath { get; set; } = string.Empty;


			[DefaultValue(true)]
			[Description("Gets or sets value which that current page selection will be shown.")]
			[StiSerializable]
			public static bool ShowPageSelection { get; set; } = true;


			[DefaultValue(/*Could not decode attribute arguments.*/)]
			[Description("Gets or sets window state of the viewer window.")]
			[StiSerializable]
			public static FormWindowState ViewerWindowState { get; set; } = (FormWindowState)2;


			[DefaultValue(true)]
			[Description("")]
			[StiSerializable]
			public static bool ScrollToEditableComponent { get; set; } = true;


			[DefaultValue(false)]
			[StiSerializable]
			public static bool LimitLengthOfEditableRichText { get; set; }

			public static object ActivePageBorderColor { get; set; }

			[DefaultValue(true)]
			[StiSerializable]
			public static bool ShowInTaskbar { get; set; } = true;


			[StiSerializable]
			public static Size MinimumSize { get; set; } = Size.Empty;


			public static Color BackgroundColor { get; set; } = Color.Empty;


			public static Icon ViewerIcon { get; set; }

			public static object ViewerWpfIcon { get; set; }

			[DefaultValue(true)]
			[StiSerializable]
			[Description("Allow the Animation component in the viewer.")]
			public static bool AllowAnimation
			{
				get
				{
					StiSettings.Load();
					return StiSettings.GetBool("Viewer", "AllowAnimation", nullValue: true);
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "AllowAnimation", value);
					StiSettings.Set("Viewer", "AnimationPlaybackType", value ? (StiAnimationPlaybackType.OnStart | StiAnimationPlaybackType.OnPreview) : StiAnimationPlaybackType.None);
					StiSettings.Save();
				}
			}

			[DefaultValue(StiAnimationPlaybackType.OnStart | StiAnimationPlaybackType.OnPreview)]
			[StiSerializable]
			public static StiAnimationPlaybackType AnimationPlaybackType
			{
				get
				{
					StiSettings.Load();
					return (StiAnimationPlaybackType)StiSettings.Get("Viewer", "AnimationPlaybackType", StiSettings.GetBool("Viewer", "AllowAnimation", nullValue: true) ? (StiAnimationPlaybackType.OnStart | StiAnimationPlaybackType.OnPreview) : StiAnimationPlaybackType.None);
				}
				set
				{
					StiSettings.Load();
					StiSettings.Set("Viewer", "AnimationPlaybackType", value);
					StiSettings.Set("Viewer", "AllowAnimation", value != StiAnimationPlaybackType.None);
					StiSettings.Save();
				}
			}
		}

		private static StiStyle interactionMouseOverStyle;

		private static StiStyle interactionSelectedStyle;

		private static StiStyle interactionMouseOverSelectedStyle;

		public static List<StiDateRangeKind> RequestFromUserDateRanges = new List<StiDateRangeKind>(new StiDateRangeKind[26]
		{
			StiDateRangeKind.Today,
			StiDateRangeKind.Tomorrow,
			StiDateRangeKind.Yesterday,
			StiDateRangeKind.Last7Days,
			StiDateRangeKind.Last14Days,
			StiDateRangeKind.Last30Days,
			StiDateRangeKind.NextWeek,
			StiDateRangeKind.CurrentWeek,
			StiDateRangeKind.PreviousWeek,
			StiDateRangeKind.NextMonth,
			StiDateRangeKind.CurrentMonth,
			StiDateRangeKind.PreviousMonth,
			StiDateRangeKind.NextQuarter,
			StiDateRangeKind.CurrentQuarter,
			StiDateRangeKind.PreviousQuarter,
			StiDateRangeKind.NextYear,
			StiDateRangeKind.CurrentYear,
			StiDateRangeKind.PreviousYear,
			StiDateRangeKind.FirstQuarter,
			StiDateRangeKind.SecondQuarter,
			StiDateRangeKind.ThirdQuarter,
			StiDateRangeKind.FourthQuarter,
			StiDateRangeKind.WeekToDate,
			StiDateRangeKind.MonthToDate,
			StiDateRangeKind.QuarterToDate,
			StiDateRangeKind.YearToDate
		});

		[DefaultValue(StiViewerRefreshingMode.DataOnly)]
		[Description("This property defines how the dashboard viewer refreshes its content during refreshing via the Report.RefreshTime property.")]
		[StiSerializable]
		public static StiViewerRefreshingMode DashboardViewerRefreshing { get; set; } = StiViewerRefreshingMode.DataOnly;


		[DefaultValue(true)]
		[Description("This property disables the ability to zoom  using a combination of Ctrl + mouse wheel.")]
		[StiSerializable]
		public static bool MouseWheelZoomingEnabled { get; set; } = true;


		[DefaultValue(null)]
		[StiSerializable]
		public static int? RequestFromUserElementsPerColumn { get; set; }

		[DefaultValue(null)]
		[StiSerializable]
		public static int? RequestFromUserPanelMaxHeight { get; set; }

		public static Color HighlightShowStateColor { get; set; } = Color.Red;


		public static Color HighlightActiveStateColor { get; set; } = Color.Red;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool DisableShortcutInViewer { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		public static bool HideStatusBarInDesigner { get; set; } = true;


		[DefaultValue(true)]
		[Description("Gets or sets value which indicates that print dialog can use async mode.")]
		[StiSerializable]
		public static bool AllowAsyncPrinting { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowDrawSegmentMode { get; set; } = true;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool AllowAcceptTabsInEditableMode { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		public static bool DisableCloseButtonInFullScreenMode { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowUseDragDrop { get; set; } = true;


		[DefaultValue(null)]
		[StiSerializable]
		public static string ViewerTitle { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		public static bool ShowSuperToolTip { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool ShowHyperlinkToolTip { get; set; } = true;


		[DefaultValue(null)]
		[StiSerializable]
		public static string ViewerTitleText { get; set; }

		[DefaultValue("{0} - {1}")]
		[StiSerializable]
		public static string ViewerTitleMask { get; set; } = "{0} - {1}";


		[DefaultValue(StiRightToLeftType.No)]
		[Description("Gets or sets a value which controls of output of objects in the right to left mode.")]
		[StiSerializable]
		public static StiRightToLeftType RightToLeft { get; set; } = StiRightToLeftType.No;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool InteractionChartOnFirstLook { get; set; } = true;


		public static bool IsRightToLeft => RightToLeft == StiRightToLeftType.Yes;

		[DefaultValue(null)]
		[StiSerializable]
		public static int? RequestFromUserDropDownWidth { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		[Description("This property is for opening sub-reports in the one tab.")]
		public static bool DrillDownSingleTab { get; set; }

		public static StiStyle InteractionMouseOverStyle
		{
			get
			{
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Expected O, but got Unknown
				object obj = interactionMouseOverStyle;
				if (obj == null)
				{
					obj = new StiStyle
					{
						Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(100, Color.Red))
					};
					interactionMouseOverStyle = (StiStyle)obj;
				}
				return (StiStyle)obj;
			}
		}

		public static StiStyle InteractionSelectedStyle
		{
			get
			{
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Expected O, but got Unknown
				object obj = interactionSelectedStyle;
				if (obj == null)
				{
					obj = new StiStyle
					{
						Brush = (StiBrush)new StiSolidBrush(Color.Red)
					};
					interactionSelectedStyle = (StiStyle)obj;
				}
				return (StiStyle)obj;
			}
		}

		public static StiStyle InteractionMouseOverSelectedStyle
		{
			get
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Expected O, but got Unknown
				object obj = interactionMouseOverSelectedStyle;
				if (obj == null)
				{
					obj = new StiStyle
					{
						Brush = (StiBrush)new StiSolidBrush(StiColorUtils.Light(Color.Red, (byte)150))
					};
					interactionMouseOverSelectedStyle = (StiStyle)obj;
				}
				return (StiStyle)obj;
			}
		}

		public static string GetViewerTitle()
		{
			return ViewerTitle ?? StiLocalization.Get("FormViewer", "title");
		}

		public static string GetViewerTitleWithMask(string file)
		{
			if (ViewerTitleText != null)
			{
				return ViewerTitleText;
			}
			if (file == null)
			{
				return GetViewerTitle();
			}
			return string.Format(ViewerTitleMask, file, GetViewerTitle());
		}
	}

	public class Web
	{
		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowUseResponseEnd { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool AllowUseResponseFlush { get; set; } = true;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool ClearResponseHeaders { get; set; }

		[DefaultValue(60)]
		[StiSerializable]
		public static int ResponseCacheTimeout { get; set; } = 60;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool AllowGCCollect { get; set; }

		[StiSerializable]
		public static Encoding WebClientEncoding
		{
			get
			{
				return StiBaseOptions.WebClientEncoding;
			}
			set
			{
				StiBaseOptions.WebClientEncoding = value;
			}
		}
	}

	public sealed class Windows
	{
		private static StiGlobalGuiStyle globalGuiStyle = StiGlobalGuiStyle.Office2013;

		[Obsolete("Please use StiOptions.Windows.GlobalGuiStyle instead Style property of StiViewerControl!")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static StiGuiStyle GuiStyle
		{
			get
			{
				if (GlobalGuiStyle == StiGlobalGuiStyle.Office2007Black)
				{
					return StiGuiStyle.Office2007Black;
				}
				if (GlobalGuiStyle == StiGlobalGuiStyle.Office2007Blue)
				{
					return StiGuiStyle.Office2007Blue;
				}
				if (GlobalGuiStyle == StiGlobalGuiStyle.Office2007Silver)
				{
					return StiGuiStyle.Office2007Silver;
				}
				return StiGuiStyle.Default;
			}
			set
			{
				switch (value)
				{
				case StiGuiStyle.Office2007Black:
					GlobalGuiStyle = StiGlobalGuiStyle.Office2007Black;
					break;
				case StiGuiStyle.Office2007Blue:
					GlobalGuiStyle = StiGlobalGuiStyle.Office2007Blue;
					break;
				case StiGuiStyle.Office2007Silver:
					GlobalGuiStyle = StiGlobalGuiStyle.Office2007Silver;
					break;
				}
			}
		}

		[DefaultValue(StiGlobalGuiStyle.Office2013)]
		[Description("Gets or sets current theme of Stimulsoft Reports application.")]
		[StiSerializable]
		[Browsable(false)]
		[Obsolete("Property 'GlobalGuiStyle' is obsoleted.")]
		public static StiGlobalGuiStyle GlobalGuiStyle
		{
			get
			{
				if (GlobalGuiStyleLoaded)
				{
					return globalGuiStyle;
				}
				StiSettings.Load();
				GlobalGuiStyleLoaded = true;
				return globalGuiStyle = (StiGlobalGuiStyle)StiSettings.Get("Windows", "GlobalGuiStyle", StiGlobalGuiStyle.Office2013);
			}
			set
			{
				if (!GlobalGuiStyleLoaded || globalGuiStyle != value)
				{
					StiSettings.Load();
					StiSettings.Set("Windows", "GlobalGuiStyle", value);
					StiGlobalGuiStyle stiGlobalGuiStyle = globalGuiStyle;
					globalGuiStyle = value;
					GlobalGuiStyleLoaded = true;
					StiSettings.Save();
					if (globalGuiStyle != stiGlobalGuiStyle)
					{
						Engine.GlobalEvents.InvokeGlobalGuiStyleChanged(null, EventArgs.Empty);
					}
				}
			}
		}

		[Obsolete("Property 'IsGuiTypeSelected' is obsoleted.")]
		public static bool IsGuiTypeSelected
		{
			get
			{
				StiSettings.Load();
				return StiSettings.GetBool("GuiHelper", "IsGuiTypeSelected", nullValue: false);
			}
			set
			{
				StiSettings.Load();
				StiSettings.Set("GuiHelper", "IsGuiTypeSelected", value);
				StiSettings.Save();
			}
		}

		public static bool GlobalGuiStyleLoaded { get; set; }

		public static StiGlobalGuiStyle? LastGlobalGuiStyle { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		public static bool UseSkinableReportRenderingWindow { get; set; }

		[DefaultValue(false)]
		[StiSerializable]
		[Description("Enables Windows 7 task bar support for report designer and report viewer applications.")]
		public static bool Windows7TaskBarSupport { get; set; }

		[Obsolete("Property 'IsOffice2010or2013Enabled' is obsoleted.")]
		public static bool IsOffice2010or2013Enabled()
		{
			if (!IsOffice2010Enabled())
			{
				return IsOffice2013Enabled();
			}
			return true;
		}

		[Obsolete("Property 'IsOffice2013Enabled' is obsoleted.")]
		public static bool IsOffice2013Enabled()
		{
			return GlobalGuiStyle == StiGlobalGuiStyle.Office2013;
		}

		[Obsolete("Property 'IsOffice2013Enabled' is obsoleted.")]
		public static bool IsOffice2013Enabled(StiGlobalGuiStyle style)
		{
			return style == StiGlobalGuiStyle.Office2013;
		}

		[Obsolete("Property 'IsOffice2013Enabled' is obsoleted.")]
		public static bool IsOffice2010Enabled()
		{
			return IsOffice2010Enabled(GlobalGuiStyle);
		}

		[Obsolete("Property 'IsOffice2010Enabled' is obsoleted.")]
		public static bool IsOffice2010Enabled(StiGlobalGuiStyle style)
		{
			if (style != StiGlobalGuiStyle.Office2010Black && style != StiGlobalGuiStyle.Office2010Blue && style != StiGlobalGuiStyle.Office2010Silver)
			{
				return style == StiGlobalGuiStyle.Windows7;
			}
			return true;
		}

		[Obsolete("Property 'IsOffice2007Enabled' is obsoleted.")]
		public static bool IsOffice2007Enabled()
		{
			return IsOffice2007Enabled(GlobalGuiStyle);
		}

		[Obsolete("Property 'IsOffice2007Enabled' is obsoleted.")]
		public static bool IsOffice2007Enabled(StiGlobalGuiStyle style)
		{
			if (style != StiGlobalGuiStyle.Office2007Black && style != StiGlobalGuiStyle.Office2007Blue && style != StiGlobalGuiStyle.Office2007Silver)
			{
				return style == StiGlobalGuiStyle.Vista;
			}
			return true;
		}

		[Obsolete("Property 'IsOffice2003Enabled' is obsoleted.")]
		public static bool IsOffice2003Enabled()
		{
			return IsOffice2003Enabled(GlobalGuiStyle);
		}

		[Obsolete("Property 'IsOffice2003Enabled' is obsoleted.")]
		public static bool IsOffice2003Enabled(StiGlobalGuiStyle style)
		{
			if (style != StiGlobalGuiStyle.Office2003Black && style != StiGlobalGuiStyle.Office2003Blue)
			{
				return style == StiGlobalGuiStyle.Office2003Silver;
			}
			return true;
		}

		[Obsolete("Property 'IsRibbonGuiEnabled' is obsoleted.")]
		public static bool IsRibbonGuiEnabled()
		{
			return IsRibbonGuiEnabled(GlobalGuiStyle);
		}

		[Obsolete("Property 'IsRibbonGuiEnabled' is obsoleted.")]
		public static bool IsRibbonGuiEnabled(StiGlobalGuiStyle style)
		{
			if (!IsOffice2007Enabled(style) && !IsOffice2010Enabled(style))
			{
				return IsOffice2013Enabled(style);
			}
			return true;
		}
	}

	public sealed class Wpf
	{
		public sealed class WpfBrowserApplication
		{
			[DefaultValue(false)]
			[StiSerializable]
			public static bool IsWpfBrowserApplication { get; set; }
		}

		public sealed class Viewer
		{
			public sealed class CollapsingImages
			{
				[DefaultValue(false)]
				[StiSerializable]
				public static bool UseCustomCollapsingImages { get; set; }

				[DefaultValue(null)]
				[StiSerializable]
				public static string CollapsedImagePath { get; set; }

				[DefaultValue(null)]
				[StiSerializable]
				public static string ExpandedImagePath { get; set; }
			}

			[DefaultValue(false)]
			[StiSerializable]
			public static bool UsePageShadow { get; set; } = true;

		}

		[Obsolete]
		public static class Themes
		{
			internal static StiWpfThemeInfo BaseOffice2013Theme { get; set; }

			public static List<StiWpfThemeInfo> ThemeList { get; set; } = new List<StiWpfThemeInfo>();


			public static string DefaultTheme => Office2013Theme;

			public static string Office2013Theme => "Office 2013";

			public static string Office2010WhiteTheme => "Office 2010 White";

			public static string Office2010BlueTheme => "Office 2010 Blue";

			public static string Office2007BlueTheme => "Office 2007 Blue";

			public static string Office2007SilverTheme => "Office 2007 Silver";

			public static string Office2007BlackTheme => "Office 2007 Black";

			public static string Office2003BlueTheme => "Office 2003 Blue";

			public static string Office2003OliveGreenTheme => "Office 2003 Olive Green";

			public static string Office2003SilverTheme => "Office 2003 Silver";

			public static string BlackTheme => "Black";
		}

		[Obsolete("This property is obsolete!")]
		[DefaultValue(StiLoadThemeMode.OnceForTheWholeApp)]
		[StiSerializable]
		public static StiLoadThemeMode LoadThemeMode = StiLoadThemeMode.OnceForTheWholeApp;

		[DefaultValue(true)]
		[StiSerializable]
		public static bool DisableAllAnimationsInViewer { get; set; } = true;


		[DefaultValue(true)]
		[StiSerializable]
		public static bool LoadTheme { get; set; } = true;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool UseGDIPrintingInWPFViewer { get; set; }

		[DefaultValue(true)]
		[StiSerializable]
		public static bool UseNewWpfPrintingMethod { get; set; } = true;


		[DefaultValue(false)]
		[StiSerializable]
		public static bool AllowPrintingProgress { get; set; }

		[Description("This property let to show in WPF designer properties of components, which unavailable in WPF.")]
		public static bool ShowAllPropertiesInWpf
		{
			get
			{
				return StiPropertiesTab.ShowAllPropertiesInWpf;
			}
			set
			{
				StiPropertiesTab.ShowAllPropertiesInWpf = value;
			}
		}

		public static bool IsGuiTypeSelected
		{
			get
			{
				StiSettings.Load();
				return StiSettings.GetBool("GuiHelper", "IsGuiTypeSelected", nullValue: false);
			}
			set
			{
				StiSettings.Load();
				StiSettings.Set("GuiHelper", "IsGuiTypeSelected", value);
				StiSettings.Save();
			}
		}

		public static string CurrentTheme => "Office 2013";

		[Obsolete]
		public static StiWpfThemeInfo GetThemeInfoForCurrentTheme()
		{
			return null;
		}
	}

	[Obsolete("Please use property 'StiOptions.Viewer' instead 'StiOptions.Preview' property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class Preview : Viewer
	{
		[Obsolete("Please use property 'StiOptions.Viewer.DotMatrix' instead 'StiOptions.Preview.DotMatrixWindow' property.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class DotMatrixWindow : DotMatrix
		{
		}

		[Obsolete("Please use property 'StiOptions.Viewer.Windows' instead 'StiOptions.Preview.Window' property.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class Window : Windows
		{
		}
	}

	public static void Load(string fileName)
	{
		if (!File.Exists(fileName))
		{
			return;
		}
		using FileStream stream = new FileStream(fileName, FileMode.Open);
		Load(stream);
	}

	public static void Load(Stream stream)
	{
		if (stream == null || stream.Length == 0L)
		{
			return;
		}
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			XmlTextReader xmlTextReader = new XmlTextReader(stream);
			xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
			xmlTextReader.Read();
			xmlTextReader.Read();
			xmlTextReader.Read();
			if (xmlTextReader.Name == "StiOptionsHelper" || xmlTextReader.Name == "StiOptions")
			{
				string propertyStr = null;
				string classTypeStr = null;
				bool flag = false;
				while (xmlTextReader.Read())
				{
					if (xmlTextReader.IsStartElement())
					{
						if (xmlTextReader.Depth == 1)
						{
							if (flag)
							{
								ApplyNewValue(propertyStr, classTypeStr, null);
							}
							propertyStr = xmlTextReader.Name;
							classTypeStr = xmlTextReader.GetAttribute("type");
							flag = true;
						}
						else
						{
							string valueStr = xmlTextReader.ReadString();
							ApplyNewValue(propertyStr, classTypeStr, valueStr);
							flag = false;
						}
					}
					xmlTextReader.Read();
				}
			}
			xmlTextReader.Close();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
			stream.Close();
		}
	}

	private static void ApplyNewValue(string propertyStr, string classTypeStr, string valueStr)
	{
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Expected O, but got Unknown
		Type type = StiTypeFinder.GetType(classTypeStr + ", Stimulsoft.Report, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
		if (type == null)
		{
			return;
		}
		PropertyInfo property = type.GetProperty(propertyStr);
		FieldInfo fieldInfo = ((property == null) ? type.GetField(propertyStr) : null);
		if (property == null && fieldInfo == null)
		{
			return;
		}
		object value = null;
		if (valueStr != null)
		{
			Type type2 = ((property != null) ? property.PropertyType : fieldInfo.FieldType);
			if (type2.IsEnum)
			{
				value = Enum.Parse(type2, valueStr);
			}
			else if (type2 == typeof(string))
			{
				value = valueStr;
			}
			else if (type2 == typeof(double))
			{
				value = double.Parse(valueStr);
			}
			else if (type2 == typeof(bool))
			{
				value = bool.Parse(valueStr);
			}
			else if (type2 == typeof(int))
			{
				value = int.Parse(valueStr);
			}
			else if (type2 == typeof(float))
			{
				value = float.Parse(valueStr);
			}
			else if (type2 == typeof(char))
			{
				value = char.Parse(valueStr);
			}
			else if (type2 == typeof(Font))
			{
				string[] array = valueStr.Split(new char[1] { ',' });
				if (array.Length == 6)
				{
					float num = float.Parse(array[1]);
					FontStyle val = (FontStyle)Enum.Parse(typeof(FontStyle), array[2]);
					GraphicsUnit val2 = (GraphicsUnit)Enum.Parse(typeof(GraphicsUnit), array[3]);
					byte b = byte.Parse(array[4]);
					bool flag = bool.Parse(array[5]);
					value = (object)new Font(array[0], num, val, val2, b, flag);
				}
			}
			else if (type2 == typeof(StiLevel?))
			{
				value = Enum.Parse(typeof(StiLevel), valueStr);
			}
			else if (type2 == typeof(Size))
			{
				string[] array2 = valueStr.Split(new char[1] { ',' });
				value = new Size(int.Parse(array2[0]), int.Parse(array2[1]));
			}
			else if (type2 == typeof(object))
			{
				object[] customAttributes = property.GetCustomAttributes(typeof(StiOptionsUniversalTypeObjectAttribute), inherit: true);
				if (customAttributes.Length != 0)
				{
					StiOptionsUniversalTypeObjectAttribute stiOptionsUniversalTypeObjectAttribute = customAttributes[0] as StiOptionsUniversalTypeObjectAttribute;
					if (stiOptionsUniversalTypeObjectAttribute.UniversalType == StiOptionsUniversalType.Enum)
					{
						value = Enum.Parse(StiTypeFinder.GetType(stiOptionsUniversalTypeObjectAttribute.Type + ", Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"), valueStr);
					}
				}
			}
		}
		if (property != null)
		{
			property.SetValue(null, value, null);
		}
		if (fieldInfo != null)
		{
			fieldInfo.SetValue(null, value);
		}
	}

	public static void Save(Stream stream)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteStartDocument(standalone: true);
			xmlTextWriter.WriteStartElement("StiOptions");
			Type typeFromHandle = typeof(StiOptions);
			MemberInfo[] members = typeFromHandle.GetMembers(BindingFlags.Public);
			MemberInfo[] array = members;
			foreach (MemberInfo memberInfo in array)
			{
				if (memberInfo.DeclaringType.IsClass)
				{
					SaveClass(xmlTextWriter, "Stimulsoft.Report.StiOptions", memberInfo.Name);
				}
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Flush();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	private static void SaveClass(XmlTextWriter tw, string parentType, string typeStr)
	{
		string text = parentType + "+" + typeStr;
		string text2 = text + ", Stimulsoft.Report, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a";
		Type type = StiTypeFinder.GetType(text2);
		if (type == null)
		{
			return;
		}
		foreach (MemberInfo item in from m in type.GetMembers(BindingFlags.Public)
			orderby m.Name
			select m into info
			where info.DeclaringType.IsClass
			select info)
		{
			SaveClass(tw, text, item.Name);
		}
		IEnumerable<MemberInfo> enumerable = from m in type.GetMembers(BindingFlags.Static | BindingFlags.Public)
			where m is PropertyInfo || m is FieldInfo
			select m;
		foreach (MemberInfo item2 in enumerable)
		{
			object actualValue = null;
			if (CheckAttributes(item2, ref actualValue))
			{
				Type propertyType = ((item2 is PropertyInfo) ? ((PropertyInfo)item2).PropertyType : ((FieldInfo)item2).FieldType);
				tw.WriteStartElement(item2.Name);
				tw.WriteAttributeString("type", text);
				SaveMember(item2, propertyType, actualValue, tw);
				tw.WriteEndElement();
			}
		}
	}

	private static void SaveMember(MemberInfo member, Type propertyType, object value, XmlTextWriter tw)
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected I4, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected I4, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected I4, but got Unknown
		if (value == null)
		{
			return;
		}
		if (propertyType.IsEnum)
		{
			tw.WriteElementString("value", ((int)value).ToString());
		}
		else if (propertyType == typeof(string) || propertyType == typeof(double) || propertyType == typeof(bool) || propertyType == typeof(int) || propertyType == typeof(float) || propertyType == typeof(char))
		{
			tw.WriteElementString("value", value.ToString());
		}
		else if (propertyType == typeof(Font))
		{
			Font val = (Font)value;
			tw.WriteElementString("value", $"{val.Name},{val.Size},{(int)val.Style},{(int)val.Unit},{val.GdiCharSet},{val.GdiVerticalFont}");
		}
		else if (propertyType == typeof(StiLevel?))
		{
			tw.WriteElementString("value", ((int)(StiLevel)value).ToString());
		}
		else if (propertyType == typeof(Size))
		{
			Size size = (Size)value;
			tw.WriteElementString("value", $"{size.Width},{size.Height}");
		}
		else
		{
			if (!(propertyType == typeof(object)))
			{
				return;
			}
			object[] customAttributes = member.GetCustomAttributes(typeof(StiOptionsUniversalTypeObjectAttribute), inherit: true);
			if (customAttributes.Length != 0)
			{
				StiOptionsUniversalTypeObjectAttribute stiOptionsUniversalTypeObjectAttribute = customAttributes[0] as StiOptionsUniversalTypeObjectAttribute;
				if (stiOptionsUniversalTypeObjectAttribute.UniversalType == StiOptionsUniversalType.Enum)
				{
					tw.WriteElementString("value", ((int)value).ToString());
				}
			}
		}
	}

	private static bool CheckAttributes(MemberInfo member, ref object actualValue)
	{
		int i = 0;
		bool flag = true;
		DefaultValueAttribute defaultValueAttribute = null;
		StiOptionsFontHelperAttribute stiOptionsFontHelperAttribute = null;
		for (object[] customAttributes = member.GetCustomAttributes(inherit: false); i < customAttributes.Length; i++)
		{
			if (customAttributes[i] is StiSerializableAttribute)
			{
				flag = false;
			}
			else if (customAttributes[i] is DefaultValueAttribute)
			{
				defaultValueAttribute = customAttributes[i] as DefaultValueAttribute;
			}
			else if (customAttributes[i] is StiOptionsFontHelperAttribute)
			{
				stiOptionsFontHelperAttribute = customAttributes[i] as StiOptionsFontHelperAttribute;
			}
		}
		if (flag)
		{
			return false;
		}
		actualValue = ((member is PropertyInfo) ? ((PropertyInfo)member).GetValue(null, null) : ((FieldInfo)member).GetValue(null));
		if (defaultValueAttribute != null)
		{
			object value = defaultValueAttribute.Value;
			if (object.Equals(value, actualValue))
			{
				return false;
			}
		}
		if (stiOptionsFontHelperAttribute != null && CheckFont(stiOptionsFontHelperAttribute.Index, (Font)/*isinst with value type is only supported in some contexts*/))
		{
			return false;
		}
		if (actualValue is Size && ((Size)actualValue).IsEmpty)
		{
			return false;
		}
		return true;
	}

	private static bool CheckFont(int index, Font font)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		if (font == null)
		{
			return false;
		}
		Font font2 = GetFont(index);
		if (font2.Name == font.Name && font2.Size == font.Size)
		{
			return font2.Style == font.Style;
		}
		return false;
	}

	private static Font GetFont(int index)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		return (Font)(index switch
		{
			0 => (object)new Font("Arial", 7f), 
			1 => (object)new Font("Arial", 10f), 
			2 => (object)new Font("Arial", 8f), 
			_ => null, 
		});
	}
}
