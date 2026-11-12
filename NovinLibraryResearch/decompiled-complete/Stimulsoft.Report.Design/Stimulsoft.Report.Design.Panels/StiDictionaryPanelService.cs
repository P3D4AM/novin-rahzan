using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Server;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Report.Check.Helper;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Dictionary;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Extensions;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Maps.Helpers;
using Stimulsoft.Report.SaveLoad;

namespace Stimulsoft.Report.Design.Panels;

[StiServiceBitmap(typeof(StiDictionaryPanelService), "Stimulsoft.Report.Design.Images.Panels.Dictionary.png")]
public class StiDictionaryPanelService : StiPanelService
{
	private bool isShowMore = true;

	private UserControl panelContent;

	private StiNoDataControl controlNoData;

	private StiDictionaryDropFilesControl controlDropFiles;

	private Bar toolBarMain;

	private ButtonItem buttonActions;

	private ButtonItem buttonNew;

	private ButtonItem buttonEdit;

	private ButtonItem buttonDelete;

	private ButtonItem buttonUp;

	private ButtonItem buttonDown;

	private ButtonItem buttonSettings;

	private LabelItem labelCommon;

	private LabelItem labelSort;

	private ButtonItem itemSortAsc;

	private ButtonItem itemSortDesc;

	private ButtonItem itemSortNone;

	private ButtonItem itemDictionaryNew;

	private ButtonItem itemDictionaryOpen;

	private ButtonItem itemDictionaryMerge;

	private ButtonItem itemDictionarySave;

	private ButtonItem itemDictionaryXmlMerge;

	private ButtonItem itemDictionaryXmlExport;

	private ButtonItem itemDictionaryXmlImport;

	private ButtonItem itemDataSourcesNew;

	private ButtonItem itemRelationsImport;

	private ButtonItem itemSynchronize;

	private ButtonItem itemMarkUsed;

	private ButtonItem itemRemoveUnused;

	private ButtonItem itemEmbedAllData;

	private ButtonItem itemConnectionNew;

	private ButtonItem itemConnectionNewFromResource;

	private ButtonItem itemDataTransformationNew;

	private ButtonItem itemBusinessObjectNew;

	private ButtonItem itemColumnNew;

	private ButtonItem itemCalcColumnNew;

	private ButtonItem itemRelationNew;

	private ButtonItem itemCategoryNew;

	private ButtonItem itemVariableNew;

	private ButtonItem itemResourceNew;

	private ButtonItem itemDataParameterNew;

	private ButtonItem contextItemEditMenu;

	private ButtonItem contextItemConnectionNew;

	private ButtonItem contextItemConnectionNewFromResource;

	private ButtonItem contextItemDataTransformationNew;

	private ButtonItem contextItemBusinessObjectNew;

	private ButtonItem contextItemColumnNew;

	private ButtonItem contextItemCalcColumnNew;

	private ButtonItem contextItemRelationNew;

	private ButtonItem contextItemCategoryNew;

	private ButtonItem contextItemVariableNew;

	private ButtonItem contextItemResourceNew;

	private ButtonItem contextItemDataParameterNew;

	private ButtonItem contextItemViewData;

	private ButtonItem contextItemSaveResource;

	private ButtonItem contextItemMakeThisRelationActive;

	private ButtonItem contextItemEdit;

	private ButtonItem contextItemProperties;

	private ButtonItem contextItemDelete;

	private ButtonItem contextItemDuplicate;

	private ButtonItem contextItemExpandAll;

	private ButtonItem contextItemCollapseAll;

	private ButtonItem itemUseAliases;

	private ButtonItem itemCreateField;

	private ButtonItem itemCreateLabel;

	private Panel panelOperation;

	private CircularProgress progressOperation;

	private Label labelOperation;

	private Timer showProgressTimer;

	private bool isProgressActive;

	private List<string> processGuidList = new List<string>();

	public override string ServiceName => StiLocalization.Get("Panels", "Dictionary");

	internal StiXmlDatabase MoveToResourceXmlDatabase { get; set; }

	internal StiDictionarySortOrder CurrentSort { get; set; }

	public override DockStyle DockStyle => (DockStyle)4;

	public override eShortcut Shortcut => eShortcut.CtrlShiftD;

	public StiDictionaryControl DictionaryControl { get; private set; }

	public bool IsEmptyDictionary
	{
		get
		{
			if (Designer == null || Designer.Report == null)
			{
				return true;
			}
			StiDictionary dictionary = Designer.Report.Dictionary;
			if (dictionary.Variables.Count == 0 && dictionary.Databases.Count == 0 && dictionary.DataSources.Count == 0 && dictionary.BusinessObjects.Count == 0)
			{
				return dictionary.Resources.Count == 0;
			}
			return false;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDuplicateForVariable
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDuplicateForVariable;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDuplicateForVariable = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDuplicateForDataSource
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDuplicateForDataSource;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDuplicateForDataSource = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDuplicateForDataColumn
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDuplicateForDataColumn;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDuplicateForDataColumn = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDuplicateForDataRelation
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDuplicateForDataRelation;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDuplicateForDataRelation = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDuplicateForDataParameter
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDuplicateForDataParameter;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDuplicateForDataParameter = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDuplicateForDataConnection
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDuplicateForDataConnection;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDuplicateForDataConnection = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDuplicateForResource
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDuplicateForResource;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDuplicateForResource = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowPropertiesForVariable
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowPropertiesForVariable;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowPropertiesForVariable = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowPropertiesForDataSource
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowPropertiesForDataSource;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowPropertiesForDataSource = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowPropertiesForBusinessObject
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowPropertiesForBusinessObject;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowPropertiesForBusinessObject = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowPropertiesForDataColumn
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowPropertiesForDataColumn;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowPropertiesForDataColumn = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowPropertiesForDataRelation
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowPropertiesForDataRelation;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowPropertiesForDataRelation = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowPropertiesForDataParameter
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowPropertiesForDataParameter;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowPropertiesForDataParameter = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowPropertiesForDataConnection
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowPropertiesForDataConnection;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowPropertiesForDataConnection = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowPropertiesForDataTransformation
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowPropertiesForDataTransformation;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowPropertiesForDataTransformation = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditForVariable
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowEditForVariable;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowEditForVariable = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditForResource
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowEditForResource;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowEditForResource = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditForDataSource
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowEditForDataSource;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowEditForDataSource = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditForBusinessObject
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowEditForBusinessObject;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowEditForBusinessObject = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditForDataColumn
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowEditForDataColumn;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowEditForDataColumn = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditForDataRelation
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowEditForDataRelation;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowEditForDataRelation = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditForDataParameter
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowEditForDataParameter;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowEditForDataParameter = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditForDataConnection
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowEditForDataConnection;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowEditForDataConnection = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditForDataTransformation
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowEditForDataTransformation;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowEditForDataTransformation = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDeleteForVariable
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDeleteForVariable;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDeleteForVariable = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDeleteForResource
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDeleteForResource;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDeleteForResource = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDeleteForDataSource
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDeleteForDataSource;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDeleteForDataSource = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDeleteForBusinessObject
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDeleteForBusinessObject;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDeleteForBusinessObject = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDeleteForDataColumn
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDeleteForDataColumn;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDeleteForDataColumn = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDeleteForDataRelation
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDeleteForDataRelation;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDeleteForDataRelation = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDeleteForDataParameter
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDeleteForDataParameter;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDeleteForDataParameter = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDeleteForDataConnection
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDeleteForDataConnection;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDeleteForDataConnection = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDeleteForDataTransformation
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDeleteForDataTransformation;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDeleteForDataTransformation = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowUseAliases
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowUseAliases;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowUseAliases = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowCreateFieldOnDoubleClick
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowCreateFieldOnDoubleClick;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowCreateFieldOnDoubleClick = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowCreateLabel
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowCreateLabel;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowCreateLabel = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowActionsButton
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowActionsButton;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowActionsButton = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowNewButton
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowNewButton;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowNewButton = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDeleteButton
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDeleteButton;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDeleteButton = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditButton
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowEditButton;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowEditButton = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowSaveResource
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowSaveResource;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowSaveResource = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowUpButton
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowUpButton;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowUpButton = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDownButton
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDownButton;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDownButton = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowSortItemsButton
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowSortItemsButton;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowSortItemsButton = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDictXmlMergeMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDictXmlMergeMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDictXmlMergeMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDictXmlExportMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDictXmlExportMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDictXmlExportMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDictXmlImportMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDictXmlImportMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDictXmlImportMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDictSaveMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDictSaveMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDictSaveMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDictMergeMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDictMergeMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDictMergeMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDictOpenMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDictOpenMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDictOpenMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDictNewMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDictNewMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDictNewMenuItem = value;
		}
	}

	[Obsolete("Please use property 'ShowConnectionNewMenuItem'.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowDatabaseNewMenuItem
	{
		get
		{
			return ShowConnectionNewMenuItem;
		}
		set
		{
			ShowConnectionNewMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowConnectionNewMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowConnectionNewMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowConnectionNewMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDataTransformationNewMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDataTransformationNewMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDataTransformationNewMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowRelationNewMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowRelationNewMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowRelationNewMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDataSourceNewMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDataSourceNewMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDataSourceNewMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowBusinessObjectNewMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowBusinessObjectNewMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowBusinessObjectNewMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowColumnNewMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowColumnNewMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowColumnNewMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDataParameterNewMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDataParameterNewMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDataParameterNewMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(false)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDataSourcesNewMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDataSourcesNewMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDataSourcesNewMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(false)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowRelationsImportMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowRelationsImportMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowRelationsImportMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowSynchronizeMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowSynchronizeMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowSynchronizeMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowViewDataMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowViewDataMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowViewDataMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowPropertiesMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowPropertiesMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowPropertiesMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDuplicateMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDuplicateMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDuplicateMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowCategoryNewMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowCategoryNewMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowCategoryNewMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowVariableNewMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowVariableNewMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowVariableNewMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowResourceNewMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowResourceNewMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowResourceNewMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowEditMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowEditMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMakeThisRelationActive
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowMakeThisRelationActive;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowMakeThisRelationActive = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDeleteMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowDeleteMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowDeleteMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowContextMenu
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowContextMenu;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowContextMenu = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(false)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMarkUsedMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowMarkUsedMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowMarkUsedMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(false)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowRemoveUnusedMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowRemoveUnusedMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowRemoveUnusedMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowCalcColumnNewMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowCalcColumnNewMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowCalcColumnNewMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowExpandAllMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowExpandAllMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowExpandAllMenuItem = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowCollapseAllMenuItem
	{
		get
		{
			return StiOptions.Designer.Panels.Dictionary.ShowCollapseAllMenuItem;
		}
		set
		{
			StiOptions.Designer.Panels.Dictionary.ShowCollapseAllMenuItem = value;
		}
	}

	private static bool IsCategoryVariable(TreeNode node)
	{
		if (!(node.Tag is StiVariable stiVariable))
		{
			return false;
		}
		if (node.Nodes.Count > 0)
		{
			return true;
		}
		if (string.IsNullOrEmpty(stiVariable.Name))
		{
			return true;
		}
		return false;
	}

	private void MoveNode(TreeNode fromNode, TreeNode toNode)
	{
		if (fromNode.Tag is StiVariable)
		{
			StiVariable stiVariable = toNode.Tag as StiVariable;
			StiVariable stiVariable2 = fromNode.Tag as StiVariable;
			if ((toNode.Tag as string != "Variables" && stiVariable == null) || stiVariable2 == stiVariable)
			{
				return;
			}
			if (!IsCategoryVariable(fromNode) && !IsCategoryVariable(toNode))
			{
				int num = Report.Dictionary.Variables.IndexOf(stiVariable2);
				int num2 = Report.Dictionary.Variables.IndexOf(stiVariable);
				Report.Dictionary.Variables.Remove(stiVariable2);
				if (toNode.Tag as string == "Variables")
				{
					stiVariable2.Category = string.Empty;
					Report.Dictionary.Variables.Add(stiVariable2);
					fromNode.Parent.Nodes.Remove(fromNode);
					toNode.Nodes.Add(fromNode);
					((TreeView)DictionaryControl.tvData).SelectedNode = fromNode;
					return;
				}
				int num3 = Report.Dictionary.Variables.IndexOf(stiVariable);
				fromNode.Parent.Nodes.Remove(fromNode);
				if (num < num2)
				{
					toNode.Parent.Nodes.Insert(toNode.Index + 1, fromNode);
					Report.Dictionary.Variables.Insert(num3 + 1, stiVariable2);
				}
				else
				{
					toNode.Parent.Nodes.Insert(toNode.Index, fromNode);
					Report.Dictionary.Variables.Insert(num3, stiVariable2);
				}
				((TreeView)DictionaryControl.tvData).SelectedNode = fromNode;
				stiVariable2.Category = stiVariable.Category;
			}
			else if (!IsCategoryVariable(fromNode) && IsCategoryVariable(toNode))
			{
				int index = Report.Dictionary.Variables.IndexOf(stiVariable);
				Report.Dictionary.Variables.Remove(stiVariable2);
				Report.Dictionary.Variables.Insert(index, stiVariable2);
				fromNode.Parent.Nodes.Remove(fromNode);
				toNode.Nodes.Add(fromNode);
				stiVariable2.Category = stiVariable.Category;
				((TreeView)DictionaryControl.tvData).SelectedNode = fromNode;
			}
			else if (IsCategoryVariable(fromNode) && IsCategoryVariable(toNode))
			{
				Report.Dictionary.Variables.MoveCategoryTo(stiVariable2.Category, stiVariable.Category);
				int firstCategoryIndex = Report.Dictionary.Variables.GetFirstCategoryIndex(stiVariable2.Category);
				int firstCategoryIndex2 = Report.Dictionary.Variables.GetFirstCategoryIndex(stiVariable.Category);
				fromNode.Parent.Nodes.Remove(fromNode);
				if (firstCategoryIndex > firstCategoryIndex2)
				{
					toNode.Parent.Nodes.Insert(toNode.Index + 1, fromNode);
				}
				else
				{
					toNode.Parent.Nodes.Insert(toNode.Index, fromNode);
				}
				((TreeView)DictionaryControl.tvData).SelectedNode = fromNode;
			}
			else
			{
				int index2 = Report.Dictionary.Variables.IndexOf(stiVariable);
				Report.Dictionary.Variables.Remove(stiVariable2);
				Report.Dictionary.Variables.Insert(index2, stiVariable2);
				if (!IsCategoryVariable(fromNode))
				{
					stiVariable2.Category = stiVariable.Category;
				}
				BuildDictionary();
				DictionaryControl.SelectNodeByTag(stiVariable2);
			}
		}
		else if (fromNode.Parent == toNode.Parent && fromNode != toNode)
		{
			((TreeView)DictionaryControl.tvData).BeginUpdate();
			int num4 = toNode.Parent.Nodes.IndexOf(toNode);
			toNode.Parent.Nodes.Remove(fromNode);
			toNode.Parent.Nodes.Insert(num4, fromNode);
			if (fromNode.Tag is StiDataSource)
			{
				StiDataRelationsCollection stiDataRelationsCollection = new StiDataRelationsCollection(Report.Dictionary);
				if (Report.Dictionary != null)
				{
					StiDataSource stiDataSource = fromNode.Tag as StiDataSource;
					foreach (StiDataRelation relation2 in Report.Dictionary.Relations)
					{
						if (relation2.ParentSource == stiDataSource || relation2.ChildSource == stiDataSource)
						{
							stiDataRelationsCollection.Insert(0, relation2);
						}
					}
				}
				int index3 = Report.Dictionary.DataSources.IndexOf(toNode.Tag as StiDataSource);
				Report.Dictionary.DataSources.Remove(fromNode.Tag as StiDataSource);
				Report.Dictionary.DataSources.Insert(index3, fromNode.Tag as StiDataSource);
				if (stiDataRelationsCollection.Count > 0)
				{
					foreach (StiDataRelation item in stiDataRelationsCollection)
					{
						Report.Dictionary.Relations.Add(item);
					}
					stiDataRelationsCollection.Clear();
				}
			}
			else if (fromNode.Tag is StiDataColumn)
			{
				StiDataSource dataSource = ((StiDataColumn)fromNode.Tag).DataSource;
				if (dataSource != null)
				{
					int index4 = dataSource.Columns.IndexOf(toNode.Tag as StiDataColumn);
					dataSource.Columns.Remove(fromNode.Tag as StiDataColumn);
					dataSource.Columns.Insert(index4, fromNode.Tag as StiDataColumn);
				}
				else
				{
					StiBusinessObject businessObject = ((StiDataColumn)fromNode.Tag).BusinessObject;
					int index5 = businessObject.Columns.IndexOf(toNode.Tag as StiDataColumn);
					businessObject.Columns.Remove(fromNode.Tag as StiDataColumn);
					businessObject.Columns.Insert(index5, fromNode.Tag as StiDataColumn);
				}
			}
			((TreeView)DictionaryControl.tvData).SelectedNode = fromNode;
			((TreeView)DictionaryControl.tvData).EndUpdate();
		}
		else if (fromNode.Parent != toNode.Parent && fromNode.Tag is StiCalcDataColumn)
		{
			((TreeView)DictionaryControl.tvData).BeginUpdate();
			int num5 = ((!(toNode.Tag is StiDataSource)) ? toNode.Parent.Nodes.IndexOf(toNode) : 0);
			fromNode.Parent.Nodes.Remove(fromNode);
			if (toNode.Tag is StiDataSource)
			{
				toNode.Nodes.Add(fromNode);
			}
			else
			{
				toNode.Parent.Nodes.Insert(num5, fromNode);
			}
			StiCalcDataColumn stiCalcDataColumn = fromNode.Tag as StiCalcDataColumn;
			StiDataSource dataSource2 = ((StiDataColumn)fromNode.Tag).DataSource;
			StiDataSource stiDataSource2 = ((toNode.Tag is StiDataSource) ? (toNode.Tag as StiDataSource) : ((StiDataColumn)toNode.Tag).DataSource);
			dataSource2.Columns.Remove(fromNode.Tag as StiCalcDataColumn);
			if (toNode.Tag is StiDataSource)
			{
				stiDataSource2.Columns.Add(fromNode.Tag as StiCalcDataColumn);
			}
			else
			{
				stiDataSource2.Columns.Insert(num5, fromNode.Tag as StiCalcDataColumn);
			}
			stiCalcDataColumn.DataSource = stiDataSource2;
			((TreeView)DictionaryControl.tvData).SelectedNode = fromNode;
			((TreeView)DictionaryControl.tvData).EndUpdate();
		}
	}

	private bool DataSourceContainDataRelation(StiDataSource dataSource, string relationName)
	{
		StiDataRelationsCollection parentRelations = dataSource.GetParentRelations();
		foreach (StiDataRelation item in parentRelations)
		{
			if (item.Name == relationName)
			{
				return true;
			}
		}
		return false;
	}

	private object GetNextNodeTag(TreeNode node)
	{
		if (node.Parent == null)
		{
			return node.Tag;
		}
		int num = node.Parent.Nodes.IndexOf(node);
		if (num < node.Parent.Nodes.Count - 1)
		{
			node = node.Parent.Nodes[num + 1];
		}
		else if (num > 0)
		{
			node = node.Parent.Nodes[num - 1];
		}
		return node.Tag;
	}

	private void DeleteFromDictionary()
	{
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Expected O, but got Unknown
		//IL_079c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Invalid comparison between Unknown and I4
		TreeNode val = ((TreeView)DictionaryControl.tvData).SelectedNode;
		if (val != null)
		{
			if (val.Tag != null)
			{
				bool flag = true;
				if (val.Tag is StiDataSource { Inherited: false } stiDataSource)
				{
					Report.Dictionary.DataSources.Remove(stiDataSource);
				}
				if (val.Tag is StiBusinessObject { Inherited: false } stiBusinessObject)
				{
					if (stiBusinessObject.ParentBusinessObject != null)
					{
						stiBusinessObject.ParentBusinessObject.BusinessObjects.Remove(stiBusinessObject);
						stiBusinessObject.ParentBusinessObject = null;
					}
					else
					{
						Report.Dictionary.BusinessObjects.Remove(stiBusinessObject);
					}
				}
				if (val.Tag is StiDataColumn { Inherited: false } stiDataColumn)
				{
					if (stiDataColumn.DataSource != null)
					{
						stiDataColumn.DataSource.Columns.Remove(stiDataColumn);
					}
					else if (stiDataColumn.BusinessObject != null)
					{
						stiDataColumn.BusinessObject.Columns.Remove(stiDataColumn);
					}
				}
				if (val.Tag is StiDataParameter { Inherited: false } stiDataParameter)
				{
					stiDataParameter.DataSource.Parameters.Remove(stiDataParameter);
				}
				if (val.Tag is StiDataRelation { Inherited: false } stiDataRelation)
				{
					Report.Dictionary.Relations.Remove(stiDataRelation);
				}
				if (val.Tag is StiDatabase { Inherited: false } stiDatabase)
				{
					foreach (TreeNode node in val.Nodes)
					{
						TreeNode val2 = node;
						if (val2.Tag is StiDataSource dataSource)
						{
							Report.Dictionary.DataSources.Remove(dataSource);
						}
					}
					Report.Dictionary.Databases.Remove(stiDatabase);
					Report.Dictionary.Connect(loadData: false);
				}
				if (val.Tag is StiVariable { Inherited: false } stiVariable)
				{
					if (!IsCategoryVariable(val))
					{
						Report.Dictionary.Variables.Remove(stiVariable);
					}
					else
					{
						Report.Dictionary.Variables.RemoveCategory(stiVariable.Category);
					}
				}
				if (val.Tag is StiResource { Inherited: false } stiResource)
				{
					List<StiDatabase> databasesUsedResource = StiUsedResourceHelper.GetDatabasesUsedResource(Report, stiResource);
					List<StiComponent> componentsUsedResource = StiUsedResourceHelper.GetComponentsUsedResource(Report, stiResource);
					if (databasesUsedResource.Count > 0 || componentsUsedResource.Count > 0)
					{
						StiDeleteUsedResourceForm stiDeleteUsedResourceForm = new StiDeleteUsedResourceForm(Designer, stiResource, databasesUsedResource, componentsUsedResource);
						try
						{
							if (Designer != null)
							{
								Designer.AddOwnedForm((Form)(object)stiDeleteUsedResourceForm);
							}
							if ((int)((Form)stiDeleteUsedResourceForm).ShowDialog() != 1)
							{
								flag = false;
							}
						}
						finally
						{
							((IDisposable)stiDeleteUsedResourceForm)?.Dispose();
						}
					}
					if (flag)
					{
						Report.Dictionary.Resources.Remove(stiResource);
					}
				}
				if (val.Tag is StiBusinessObjectCategory)
				{
					string category = ((StiBusinessObjectCategory)val.Tag).Category;
					int num = 0;
					while (num < Report.Dictionary.BusinessObjects.Count)
					{
						StiBusinessObject stiBusinessObject2 = Report.Dictionary.BusinessObjects[num];
						if (stiBusinessObject2.Category == category)
						{
							Report.Dictionary.BusinessObjects.RemoveAt(num);
						}
						else
						{
							num++;
						}
					}
				}
				if (val.Tag is string)
				{
					if ((string)val.Tag == "DataSources")
					{
						Report.Dictionary.Databases.Clear();
						Report.Dictionary.DataSources.Clear();
						Report.Dictionary.Relations.Clear();
					}
					else if ((string)val.Tag == "Resources")
					{
						Report.Dictionary.Resources.Clear();
					}
					else if ((string)val.Tag == "BusinessObjects")
					{
						Report.Dictionary.BusinessObjects.Clear();
					}
				}
				if (flag)
				{
					object nextNodeTag = GetNextNodeTag(val);
					BuildDictionary();
					DictionaryControl.SelectNodeByTag(nextNodeTag);
					StiCacheCleaner.Clean(Designer?.Report);
				}
			}
			else
			{
				if (val.Text == StiLocalization.Get("PropertyMain", "Parameters"))
				{
					TreeNode val3 = val.NextVisibleNode;
					if (val3 == null)
					{
						val3 = val.FirstNode;
					}
					TreeNode val4 = val.Nodes[0];
					if (val4.Tag is StiDataParameter { Inherited: false } stiDataParameter2)
					{
						((StiSqlSource)stiDataParameter2.DataSource).Parameters.Clear();
					}
					BuildDictionary();
					((TreeView)DictionaryControl.tvData).SelectedNode = val3;
					StiCacheCleaner.Clean(Designer?.Report);
					return;
				}
				if (((TreeView)DictionaryControl.tvData).Nodes.Contains(val))
				{
					TreeNode val5 = val.NextVisibleNode;
					if (val5 == null)
					{
						val5 = val.FirstNode;
					}
					if (val.ImageKey == "DataStore")
					{
						foreach (TreeNode node2 in val.Nodes)
						{
							TreeNode val6 = node2;
							if (val6.Tag is StiDataSource dataSource2)
							{
								Report.Dictionary.DataSources.Remove(dataSource2);
							}
						}
					}
					else if (val.Text == StiLocalization.Get("PropertyMain", "CategoryConnections"))
					{
						int num2 = 0;
						while (num2 < Report.Dictionary.Databases.Count)
						{
							StiDatabase stiDatabase2 = Report.Dictionary.Databases[num2];
							if (!stiDatabase2.Inherited)
							{
								Report.Dictionary.Databases.RemoveAt(num2);
							}
							else
							{
								num2++;
							}
						}
						Report.Dictionary.Connect(loadData: false);
					}
					else if (val.Text == StiLocalization.Get("PropertyMain", "Variables"))
					{
						int num2 = 0;
						while (num2 < Report.Dictionary.Variables.Count)
						{
							StiVariable stiVariable2 = Report.Dictionary.Variables[num2];
							if (!stiVariable2.Inherited)
							{
								Report.Dictionary.Variables.RemoveAt(num2);
							}
							else
							{
								num2++;
							}
						}
					}
					BuildDictionary();
					((TreeView)DictionaryControl.tvData).SelectedNode = val5;
					StiCacheCleaner.Clean(Designer?.Report);
				}
				else
				{
					TreeNode parent = ((TreeView)DictionaryControl.tvData).SelectedNode.Parent;
					if (parent != null && ((TreeView)DictionaryControl.tvData).Nodes.Contains(parent) && parent.Text == StiLocalization.Get("PropertyMain", "DataSources"))
					{
						Hashtable hashtable = new Hashtable();
						foreach (TreeNode node3 in ((TreeView)DictionaryControl.tvData).SelectedNode.Nodes)
						{
							TreeNode val7 = node3;
							if (val7.Tag is StiDataSource { Inherited: false } stiDataSource2)
							{
								hashtable[stiDataSource2] = stiDataSource2;
								Report.Dictionary.DataSources.Remove(stiDataSource2);
							}
						}
						int num3 = 0;
						while (num3 < Report.Dictionary.Relations.Count)
						{
							StiDataRelation stiDataRelation2 = Report.Dictionary.Relations[num3];
							if (hashtable[stiDataRelation2.ParentSource] != null || hashtable[stiDataRelation2.ChildSource] != null)
							{
								Report.Dictionary.Relations.RemoveAt(num3);
							}
							else
							{
								num3++;
							}
						}
					}
					int num4 = parent.Nodes.IndexOf(val);
					if (num4 < parent.Nodes.Count - 1)
					{
						val = parent.Nodes[num4 + 1];
					}
					else if (num4 > 0)
					{
						val = parent.Nodes[num4 - 1];
					}
					((TreeView)DictionaryControl.tvData).SelectedNode = val;
					BuildDictionary();
					StiCacheCleaner.Clean(Designer?.Report);
				}
			}
		}
		DictionaryCheck();
	}

	private StiSqlSource GetSqlSource(TreeNode node)
	{
		object tag = node.Tag;
		StiSqlSource result = null;
		if (tag is StiDataSource)
		{
			result = tag as StiSqlSource;
		}
		else if (tag is StiDataParameter)
		{
			result = ((StiDataParameter)tag).DataSource as StiSqlSource;
		}
		else if (tag is StiDataColumn)
		{
			result = ((StiDataColumn)tag).DataSource as StiSqlSource;
		}
		else if (node.Text == StiLocalization.Get("PropertyMain", "Parameters"))
		{
			TreeNode parent = node.Parent;
			result = parent.Tag as StiSqlSource;
		}
		return result;
	}

	private void DictionaryCheck()
	{
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected O, but got Unknown
		try
		{
			TreeNode selectedNode = ((TreeView)DictionaryControl.tvData).SelectedNode;
			IStiInherited stiInherited = null;
			if (selectedNode != null)
			{
				stiInherited = ((TreeView)DictionaryControl.tvData).SelectedNode.Tag as IStiInherited;
			}
			bool flag = stiInherited?.Inherited ?? false;
			ButtonItem buttonItem = contextItemProperties;
			TreeNode selectedNode2 = ((TreeView)DictionaryControl.tvData).SelectedNode;
			buttonItem.Enabled = ((selectedNode2 != null) ? selectedNode2.Tag : null) != null && !flag;
			ButtonItem buttonItem2 = contextItemDuplicate;
			TreeNode selectedNode3 = ((TreeView)DictionaryControl.tvData).SelectedNode;
			buttonItem2.Enabled = ((selectedNode3 != null) ? selectedNode3.Tag : null) != null && !flag;
			if (contextItemDuplicate.Enabled)
			{
				object obj = ((selectedNode != null) ? selectedNode.Tag : null);
				if (obj is StiVariable && ShowDuplicateForVariable)
				{
					SetDictionaryDuplicate(visible: true);
				}
				else if (obj is StiDataSource && ShowDuplicateForDataSource)
				{
					SetDictionaryDuplicate(visible: true);
				}
				else if (obj is StiDataRelation && ShowDuplicateForDataRelation)
				{
					SetDictionaryDuplicate(visible: true);
				}
				else if (obj is StiDatabase && ShowDuplicateForDataConnection)
				{
					SetDictionaryDuplicate(visible: true);
				}
				else if (obj is StiResource && ShowDuplicateForResource)
				{
					SetDictionaryDuplicate(visible: true);
				}
				else
				{
					SetDictionaryDuplicate(visible: false);
				}
			}
			else
			{
				SetDictionaryDuplicate(visible: false);
			}
			if (contextItemProperties.Enabled)
			{
				object obj2 = ((selectedNode != null) ? selectedNode.Tag : null);
				if (obj2 is StiVariable && ShowPropertiesForVariable)
				{
					SetDictionaryProperties(visible: true);
				}
				else if (obj2 is StiDataSource && ShowPropertiesForDataSource)
				{
					SetDictionaryProperties(visible: true);
				}
				else if (obj2 is StiBusinessObject && ShowPropertiesForBusinessObject)
				{
					SetDictionaryProperties(visible: true);
				}
				else if (obj2 is StiDataColumn && ShowPropertiesForDataColumn)
				{
					SetDictionaryProperties(visible: true);
				}
				else if (obj2 is StiDataRelation && ShowPropertiesForDataRelation)
				{
					SetDictionaryProperties(visible: true);
				}
				else if (obj2 is StiDataParameter && ShowPropertiesForDataParameter)
				{
					SetDictionaryProperties(visible: true);
				}
				else if (obj2 is StiDatabase && ShowPropertiesForDataConnection)
				{
					SetDictionaryProperties(visible: true);
				}
				else if (obj2 is StiDataTransformation && ShowPropertiesForDataTransformation)
				{
					SetDictionaryProperties(visible: true);
				}
				else
				{
					SetDictionaryProperties(visible: false);
				}
			}
			else
			{
				SetDictionaryProperties(visible: false);
			}
			contextItemExpandAll.Visible = !IsEmptyDictionary && ShowExpandAllMenuItem;
			contextItemCollapseAll.Visible = !IsEmptyDictionary && ShowCollapseAllMenuItem;
			if (selectedNode != null && (selectedNode.Tag is StiDataColumn || selectedNode.Tag is StiDataSource || selectedNode.Tag is StiVariable))
			{
				if (CurrentSort == StiDictionarySortOrder.None)
				{
					int num = 0;
					if (selectedNode.Tag is StiDataColumn)
					{
						foreach (TreeNode node in selectedNode.Parent.Nodes)
						{
							TreeNode val = node;
							if (!(val.Tag is StiDataColumn))
							{
								num++;
								continue;
							}
							break;
						}
					}
					buttonUp.Enabled = selectedNode.Index > num;
					buttonDown.Enabled = selectedNode.Index < selectedNode.Parent.Nodes.Count - 1;
				}
				else
				{
					buttonUp.Enabled = false;
					buttonDown.Enabled = false;
				}
			}
			else
			{
				buttonUp.Enabled = false;
				buttonDown.Enabled = false;
			}
			if (selectedNode != null)
			{
				bool flag2 = false;
				if (((TreeView)DictionaryControl.tvData).Nodes.Contains(selectedNode))
				{
					if (selectedNode.Text == StiLocalization.Get("PropertyMain", "SystemVariables") || selectedNode.Text == StiLocalization.Get("PropertyMain", "AggregateFunctions"))
					{
						SetDictionaryDelete(visible: false);
						SetDictionaryProperties(visible: false);
					}
					else
					{
						flag2 = true;
					}
				}
				else
				{
					flag2 = true;
				}
				if (flag2)
				{
					object tag = selectedNode.Tag;
					if (tag is StiVariable && ShowDeleteForVariable)
					{
						SetDictionaryDelete(!flag);
					}
					else if (tag is StiResource && ShowDeleteForResource)
					{
						SetDictionaryDelete(!flag);
					}
					else if (tag is StiDataSource && ShowDeleteForDataSource)
					{
						SetDictionaryDelete(!flag);
					}
					else if (tag is StiBusinessObject && ShowDeleteForBusinessObject)
					{
						SetDictionaryDelete(!flag);
					}
					else if (tag is StiBusinessObjectCategory && ShowDeleteForBusinessObject)
					{
						SetDictionaryDelete(!flag);
					}
					else if (tag is StiDataColumn && ShowDeleteForDataColumn)
					{
						SetDictionaryDelete(!flag);
					}
					else if (tag is StiDataRelation && ShowDeleteForDataRelation)
					{
						SetDictionaryDelete(!flag);
					}
					else if (tag is StiDataParameter && ShowDeleteForDataParameter)
					{
						SetDictionaryDelete(!flag);
					}
					else if (tag is StiDatabase && ShowDeleteForDataConnection)
					{
						SetDictionaryDelete(!flag);
					}
					else if (tag is StiDataTransformation && ShowDeleteForDataTransformation)
					{
						SetDictionaryDelete(!flag);
					}
					else if (selectedNode.Text == StiLocalization.Get("PropertyMain", "DataSources") && ShowDeleteForDataSource)
					{
						SetDictionaryDelete(!flag);
					}
					else if (selectedNode.Text == StiLocalization.Get("Report", "BusinessObjects") && ShowDeleteForBusinessObject)
					{
						SetDictionaryDelete(!flag);
					}
					else if (selectedNode.Text == StiLocalization.Get("PropertyMain", "Variables") && ShowDeleteForVariable)
					{
						SetDictionaryDelete(!flag);
					}
					else if (selectedNode.Text == StiLocalization.Get("PropertyMain", "Resources") && ShowDeleteForResource)
					{
						SetDictionaryDelete(!flag);
					}
					else
					{
						SetDictionaryDelete(visible: false);
					}
				}
			}
			else
			{
				SetDictionaryDelete(visible: false);
			}
			bool flag3 = false;
			object obj3 = ((selectedNode != null) ? selectedNode.Tag : null);
			if (obj3 is StiVariable && ShowEditForVariable)
			{
				SetDictionaryEdit(!flag);
			}
			else if (obj3 is StiResource && ShowEditForResource)
			{
				flag3 = true;
				SetDictionaryEdit(!flag);
			}
			else if (obj3 is StiDataSource && ShowEditForDataSource)
			{
				SetDictionaryEdit(!flag);
			}
			else if (obj3 is StiBusinessObject && ShowEditForBusinessObject)
			{
				SetDictionaryEdit(!flag);
			}
			else if (obj3 is StiBusinessObjectCategory && ShowEditForBusinessObject)
			{
				SetDictionaryEdit(!flag);
			}
			else if (obj3 is StiDataColumn && ShowEditForDataColumn)
			{
				SetDictionaryEdit(!flag);
			}
			else if (obj3 is StiDataRelation && ShowEditForDataRelation)
			{
				SetDictionaryEdit(!flag);
			}
			else if (obj3 is StiDataParameter && ShowEditForDataParameter)
			{
				SetDictionaryEdit(!flag);
			}
			else if (obj3 is StiDatabase && ShowEditForDataConnection)
			{
				SetDictionaryEdit(!flag);
			}
			else if (obj3 is StiDataTransformation && ShowEditForDataTransformation)
			{
				SetDictionaryEdit(!flag);
			}
			else
			{
				SetDictionaryEdit(visible: false);
			}
			contextItemSaveResource.Visible = ShowSaveResource && flag3;
			if (flag3)
			{
				StiResource stiResource = (StiResource)obj3;
				if (StiResourceTypeHelper.PossibleConvertToDataSource(stiResource.Type))
				{
					string dataSourceIconName = StiResourceTypeHelper.GetDataSourceIconName(stiResource.Type);
					string text = StiLocalization.Get("MainMenu", "menuEditDataSourceNew").Replace("...", string.Empty) + " [" + stiResource.Name + "]";
					Image image = null;
					switch (dataSourceIconName)
					{
					case "Csv":
						image = StiReportDesignImages.Data.Csv();
						break;
					case "Dbf":
						image = StiReportDesignImages.Data.DBase();
						break;
					case "Excel":
						image = StiReportDesignImages.Data.Excel();
						break;
					case "Json":
						image = StiReportDesignImages.Data.Json();
						break;
					case "Xml":
						image = StiReportDesignImages.Data.Xml();
						break;
					}
					if (contextItemConnectionNewFromResource != null)
					{
						contextItemConnectionNewFromResource.Visible = true;
						contextItemConnectionNewFromResource.Text = text;
						contextItemConnectionNewFromResource.Image = image;
					}
					if (itemConnectionNewFromResource != null)
					{
						itemConnectionNewFromResource.Visible = true;
						itemConnectionNewFromResource.Text = text;
						itemConnectionNewFromResource.Image = image;
					}
				}
				else
				{
					if (contextItemConnectionNewFromResource != null)
					{
						contextItemConnectionNewFromResource.Visible = false;
					}
					if (itemConnectionNewFromResource != null)
					{
						itemConnectionNewFromResource.Visible = false;
					}
				}
			}
			else
			{
				if (contextItemConnectionNewFromResource != null)
				{
					contextItemConnectionNewFromResource.Visible = false;
				}
				if (itemConnectionNewFromResource != null)
				{
					itemConnectionNewFromResource.Visible = false;
				}
			}
			object obj4 = null;
			if (selectedNode != null)
			{
				obj4 = selectedNode.Tag;
			}
			if (obj4 is StiDataRelation)
			{
				SetMakeThisRelationActive(!((StiDataRelation)obj4).Active);
			}
			else
			{
				SetMakeThisRelationActive(visible: false);
			}
			if (obj4 is StiDataTransformation || obj4 is StiDataTransformationColumn)
			{
				SetDictionaryColumnNew(columnVisible: true, calcColumnVisible: false);
			}
			else if (obj4 is StiDataSource || obj4 is StiBusinessObject || obj4 is StiDataColumn)
			{
				SetDictionaryColumnNew(columnVisible: true, calcColumnVisible: true);
			}
			else
			{
				SetDictionaryColumnNew(columnVisible: false, calcColumnVisible: false);
			}
			if (obj4 is StiSqlSource || obj4 is StiDataColumn || obj4 is StiDataParameter || (selectedNode != null && selectedNode.Text == StiLocalization.Get("PropertyMain", "Parameters")))
			{
				StiSqlSource sqlSource = GetSqlSource(((TreeView)DictionaryControl.tvData).SelectedNode);
				SetDictionaryDataParameterNew(sqlSource != null);
			}
			else
			{
				SetDictionaryDataParameterNew(visible: false);
			}
			if (obj4 is StiDataSource || obj4 is StiBusinessObject)
			{
				SetDictionaryViewData(visible: true);
			}
			else
			{
				SetDictionaryViewData(visible: false);
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode == null)
			{
				return;
			}
			string text2 = null;
			StiDataType dataType = StiDataType.BusinessObject;
			if (((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiDataColumn stiDataColumn)
			{
				if (stiDataColumn.DataSource != null)
				{
					text2 = stiDataColumn.DataSource.Name + "." + stiDataColumn.Name;
				}
				else if (stiDataColumn.BusinessObject != null)
				{
					text2 = stiDataColumn.BusinessObject.Name + "." + stiDataColumn.Name;
				}
				dataType = StiDataType.DataColumn;
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiDataSource stiDataSource)
			{
				text2 = stiDataSource.Name;
				dataType = StiDataType.DataSource;
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiDataRelation stiDataRelation)
			{
				text2 = stiDataRelation.Name;
				dataType = StiDataType.DataRelation;
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiDatabase stiDatabase)
			{
				text2 = stiDatabase.Name;
				dataType = StiDataType.Database;
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiVariable stiVariable)
			{
				text2 = stiVariable.Name;
				dataType = StiDataType.Variable;
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiResource stiResource2)
			{
				text2 = stiResource2.Name;
				dataType = StiDataType.Resource;
			}
			if (text2 == null)
			{
				return;
			}
			if (buttonEdit.Enabled)
			{
				SetDictionaryEdit(Designer.Report.Dictionary.Restrictions.IsAllowEdit(text2, dataType));
			}
			if (buttonDelete.Enabled)
			{
				SetDictionaryDelete(Designer.Report.Dictionary.Restrictions.IsAllowDelete(text2, dataType));
			}
			if (CurrentSort == StiDictionarySortOrder.None)
			{
				bool enabled = Designer.Report.Dictionary.Restrictions.IsAllowMove(text2, dataType);
				if (buttonUp.Enabled)
				{
					buttonUp.Enabled = enabled;
				}
				if (buttonDown.Enabled)
				{
					buttonDown.Enabled = enabled;
				}
			}
			else
			{
				buttonUp.Enabled = false;
				buttonDown.Enabled = false;
			}
		}
		catch
		{
		}
	}

	private void BusinessObjectsCheck()
	{
		contextItemBusinessObjectNew.Visible = Report != null && Report.EngineVersion == StiEngineVersion.EngineV2 && ShowBusinessObjectNewMenuItem;
		itemBusinessObjectNew.Visible = Report != null && Report.EngineVersion == StiEngineVersion.EngineV2 && ShowBusinessObjectNewMenuItem;
		if (StiOptions.Designer.IsBeginnerDesigner || StiOptions.Designer.IsBICreatorDesigner)
		{
			contextItemBusinessObjectNew.Visible = false;
			itemBusinessObjectNew.Visible = false;
		}
	}

	private void RelationCheck()
	{
		ButtonItem buttonItem = itemRelationNew;
		bool visible = (contextItemRelationNew.Visible = Report != null && !IsEmptyDictionary && ShowRelationNewMenuItem);
		buttonItem.Visible = visible;
	}

	public void SetDictionaryDelete(bool visible)
	{
		buttonDelete.Enabled = visible && ShowDeleteButton;
		contextItemDelete.Visible = visible && ShowDeleteMenuItem;
	}

	public void SetDictionaryDuplicate(bool visible)
	{
		contextItemDuplicate.Visible = visible;
	}

	public void SetDictionaryProperties(bool visible)
	{
		contextItemProperties.Visible = visible;
	}

	public void SetDictionaryEdit(bool visible)
	{
		buttonEdit.Enabled = visible && ShowEditButton;
		contextItemEdit.Visible = visible && ShowEditMenuItem;
	}

	private void SetMakeThisRelationActive(bool visible)
	{
		contextItemMakeThisRelationActive.Visible = visible && ShowMakeThisRelationActive;
	}

	public void SetDictionaryColumnNew(bool columnVisible, bool calcColumnVisible)
	{
		ButtonItem buttonItem = itemColumnNew;
		bool visible = (contextItemColumnNew.Visible = columnVisible && ShowColumnNewMenuItem);
		buttonItem.Visible = visible;
		ButtonItem buttonItem2 = itemCalcColumnNew;
		visible = (contextItemCalcColumnNew.Visible = calcColumnVisible && ShowColumnNewMenuItem);
		buttonItem2.Visible = visible;
	}

	public void SetDictionaryDataParameterNew(bool visible)
	{
		ButtonItem buttonItem = itemDataParameterNew;
		bool visible2 = (contextItemDataParameterNew.Visible = visible && ShowDataParameterNewMenuItem);
		buttonItem.Visible = visible2;
	}

	[Obsolete("StiDictionaryPanelService.SetDictionaryDatabaseNew method is obsolete! Please use StiDictionaryPanelService.SetDictionaryConnectionNew.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDictionaryDatabaseNew(bool visible)
	{
		SetDictionaryConnectionNew(visible);
	}

	public void SetDictionaryConnectionNew(bool visible)
	{
		ButtonItem buttonItem = itemConnectionNew;
		bool visible2 = (contextItemConnectionNew.Visible = visible && ShowConnectionNewMenuItem);
		buttonItem.Visible = visible2;
	}

	public void SetDictionaryDataTransformationNew(bool visible)
	{
		ButtonItem buttonItem = itemDataTransformationNew;
		bool visible2 = (contextItemDataTransformationNew.Visible = visible && ShowDataTransformationNewMenuItem);
		buttonItem.Visible = visible2;
	}

	public void SetDictionaryViewData(bool visible)
	{
		contextItemViewData.Visible = visible && ShowViewDataMenuItem;
	}

	public virtual void InvokeConnectionNewFromResouce()
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Invalid comparison between Unknown and I4
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			TreeNode selectedNode = ((TreeView)DictionaryControl.tvData).SelectedNode;
			if (selectedNode == null || !(selectedNode.Tag is StiResource))
			{
				return;
			}
			StiResource stiResource = (StiResource)selectedNode.Tag;
			StiFileDatabase stiFileDatabase = stiResource.CreateFileDatabase();
			if (stiFileDatabase == null)
			{
				return;
			}
			string alias = (stiFileDatabase.Name = StiNameCreation.CreateConnectionName(Designer.Report, stiResource.Name.Replace(" ", string.Empty)));
			stiFileDatabase.Alias = alias;
			StiCreateNewConnectionResult stiCreateNewConnectionResult = null;
			if ((int)stiFileDatabase.Edit((Report == null) ? null : Report.Dictionary, newDatabase: true) == 1)
			{
				stiCreateNewConnectionResult = new StiCreateNewConnectionResult
				{
					Database = stiFileDatabase,
					SkipSchemaWizard = false
				};
			}
			if (stiCreateNewConnectionResult == null)
			{
				return;
			}
			StiDictionary dictionary = Designer.Report.Dictionary;
			Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "DatabaseNew"));
			if (StiNameCreation.IsConnectionNameExists(Designer.Report, stiCreateNewConnectionResult.Database.Name))
			{
				string text2 = StiNameCreation.CreateConnectionName(Designer.Report, stiCreateNewConnectionResult.Database.Name);
				if (stiCreateNewConnectionResult.Database.Name == stiCreateNewConnectionResult.Database.Alias)
				{
					stiCreateNewConnectionResult.Database.Alias = text2;
				}
				stiCreateNewConnectionResult.Database.Name = text2;
			}
			dictionary.Databases.Add(stiCreateNewConnectionResult.Database);
			dictionary.Connect(loadData: false);
			BuildDictionary();
			DictionaryControl.SelectNodeByTag(stiCreateNewConnectionResult.Database);
			if (stiCreateNewConnectionResult.SkipSchemaWizard && stiCreateNewConnectionResult.Database is StiSqlDatabase)
			{
				StiReportNewConnection.InvokeDataStoreSourceEdit(Designer, Designer.Report, (StiSqlDatabase)stiCreateNewConnectionResult.Database);
			}
			else
			{
				InvokeSelectDataForm(stiCreateNewConnectionResult.Database);
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	public virtual async Task InvokeConnectionNewFromResouceAsync()
	{
		_ = 2;
		try
		{
			TreeNode selectedNode = ((TreeView)DictionaryControl.tvData).SelectedNode;
			if (selectedNode == null || !(selectedNode.Tag is StiResource))
			{
				return;
			}
			StiResource stiResource = (StiResource)selectedNode.Tag;
			StiFileDatabase database = stiResource.CreateFileDatabase();
			if (database == null)
			{
				return;
			}
			string alias = (database.Name = StiNameCreation.CreateConnectionName(Designer.Report, stiResource.Name.Replace(" ", string.Empty)));
			database.Alias = alias;
			StiCreateNewConnectionResult result = null;
			if ((int)(await database.EditAsync((Report == null) ? null : Report.Dictionary, newDatabase: true)) == 1)
			{
				result = new StiCreateNewConnectionResult
				{
					Database = database,
					SkipSchemaWizard = false
				};
			}
			if (result == null)
			{
				return;
			}
			try
			{
				StiDictionaryPanelHelper.ShowProgress(Designer, Loc.Get("DesignerFx", "LoadingData"));
				await Designer.UndoRedoSaveAsync(StiLocalization.Get("FormDictionaryDesigner", "DatabaseNew"));
				if (StiNameCreation.IsConnectionNameExists(Designer.Report, result.Database.Name))
				{
					string text2 = StiNameCreation.CreateConnectionName(Designer.Report, result.Database.Name);
					if (result.Database.Name == result.Database.Alias)
					{
						result.Database.Alias = text2;
					}
					result.Database.Name = text2;
				}
				Designer.Report.Dictionary.Databases.Add(result.Database);
				await Designer.Report.Dictionary.ConnectAsync(loadData: false);
				BuildDictionary();
			}
			finally
			{
				StiDictionaryPanelHelper.HideProgress(Designer);
			}
			DictionaryControl.SelectNodeByTag(result.Database);
			if (result.SkipSchemaWizard && result.Database is StiSqlDatabase)
			{
				StiReportNewConnection.InvokeDataStoreSourceEdit(Designer, Designer.Report, (StiSqlDatabase)result.Database);
			}
			else
			{
				InvokeSelectDataForm(result.Database);
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private DialogResult InvokeSelectDataForm(StiDatabase database)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		StiSelectDataForm stiSelectDataForm = new StiSelectDataForm(Designer.Report, database);
		Designer.AddOwnedForm((Form)(object)stiSelectDataForm);
		if ((int)((Form)stiSelectDataForm).ShowDialog() == 1)
		{
			Designer.GetDictionaryService().BuildDictionary();
			TreeNode selectedNode = ((TreeView)DictionaryControl.tvData).SelectedNode;
			if (selectedNode != null && selectedNode.Nodes.Count > 0)
			{
				((TreeView)DictionaryControl.tvData).SelectedNode = selectedNode.Nodes[0];
			}
		}
		return ((Form)stiSelectDataForm).DialogResult;
	}

	public virtual async Task<StiDatabase> InvokeConnectionNewAsync()
	{
		try
		{
			MoveToResourceXmlDatabase = null;
			return ProcessingResultNewConnection(await StiReportNewConnection.CreateNewConnectionAsync(Designer, Designer.Report));
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
		return null;
	}

	public virtual StiDatabase InvokeConnectionNew()
	{
		try
		{
			MoveToResourceXmlDatabase = null;
			StiCreateNewConnectionResult result = StiReportNewConnection.CreateNewConnection(Designer, Designer.Report);
			return ProcessingResultNewConnection(result);
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
		return null;
	}

	private StiDatabase ProcessingResultNewConnection(StiCreateNewConnectionResult result)
	{
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Invalid comparison between Unknown and I4
		try
		{
			if (result != null)
			{
				StiDatabase stiDatabase = result.Database;
				if (MoveToResourceXmlDatabase != null)
				{
					stiDatabase = MoveToResourceXmlDatabase;
					MoveToResourceXmlDatabase = null;
				}
				StiDictionary dictionary = Designer.Report.Dictionary;
				Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "DatabaseNew"));
				if (StiNameCreation.IsConnectionNameExists(Designer.Report, stiDatabase.Name))
				{
					string text = StiNameCreation.CreateConnectionName(Designer.Report, stiDatabase.Name);
					if (stiDatabase.Name == stiDatabase.Alias)
					{
						stiDatabase.Alias = text;
					}
					stiDatabase.Name = text;
				}
				dictionary.Databases.Add(stiDatabase);
				dictionary.Connect(loadData: false);
				BuildDictionary();
				DictionaryControl.SelectNodeByTag(stiDatabase);
				if (result.SkipSchemaWizard && stiDatabase is StiSqlDatabase)
				{
					StiReportNewConnection.InvokeDataStoreSourceEdit(Designer, Designer.Report, (StiSqlDatabase)stiDatabase);
				}
				else
				{
					StiSelectDataForm stiSelectDataForm = new StiSelectDataForm(Designer.Report, stiDatabase);
					Designer.AddOwnedForm((Form)(object)stiSelectDataForm);
					if ((int)((Form)stiSelectDataForm).ShowDialog() == 1)
					{
						Designer.GetDictionaryService().BuildDictionary();
						TreeNode selectedNode = ((TreeView)DictionaryControl.tvData).SelectedNode;
						if (selectedNode != null && selectedNode.Nodes.Count > 0)
						{
							((TreeView)DictionaryControl.tvData).SelectedNode = selectedNode.Nodes[0];
						}
					}
				}
				return stiDatabase;
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
		return null;
	}

	public virtual bool InvokeConnectionEdit(StiDatabase database)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		MoveToResourceXmlDatabase = null;
		Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "DatabaseEdit"));
		if ((int)database.Edit(Report.Dictionary, newDatabase: false) == 1)
		{
			if (MoveToResourceXmlDatabase != null)
			{
				if (Report.Dictionary.Databases.Contains(database))
				{
					Report.Dictionary.Databases.Remove(database);
				}
				Report.Dictionary.Databases.Add(MoveToResourceXmlDatabase);
				database = MoveToResourceXmlDatabase;
				MoveToResourceXmlDatabase = null;
			}
			StiRecentConnections.RecentConnections = null;
			StiRecentConnections stiRecentConnections = new StiRecentConnections();
			stiRecentConnections.AddAndSave(database);
			Report.Dictionary.Connect(loadData: false);
			BuildDictionary();
			return true;
		}
		if (Designer.UndoRedo != null && Designer.UndoRedo.CanUndo)
		{
			Designer.UndoRedo.UndoStackPop();
		}
		return false;
	}

	public virtual void InvokeDataTransformationNew()
	{
		string text = StiNameCreation.CreateName(Report, Loc.Get("PropertyMain", "Data"));
		StiDataTransformation component = new StiDataTransformation
		{
			NameInSource = Loc.Get("PropertyMain", "Category"),
			Name = text,
			Alias = text,
			Dictionary = Report.Dictionary
		};
		StiInplaceDesigner.Close();
		Stimulsoft.Report.Components.Design.StiDataTransformationDesigner stiDataTransformationDesigner = new Stimulsoft.Report.Components.Design.StiDataTransformationDesigner(Report.Designer.DesignerControl);
		stiDataTransformationDesigner.DesignComponent(component);
	}

	public virtual void InvokeDataTransformationEdit(StiDataTransformation dataTransformation)
	{
		StiInplaceDesigner.Close();
		Stimulsoft.Report.Components.Design.StiDataTransformationDesigner stiDataTransformationDesigner = new Stimulsoft.Report.Components.Design.StiDataTransformationDesigner(Report.Designer.DesignerControl);
		stiDataTransformationDesigner.DesignComponent(dataTransformation);
	}

	public virtual void InvokeBusinessObjectNew()
	{
		ItemBusinessObjectNew_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeColumnNew()
	{
		ItemColumnNew_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeCalcColumnNew()
	{
		ItemCalcColumnNew_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeDataParameterNew()
	{
		ItemDataParameterNew_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeRelationNew()
	{
		ItemRelationNew_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeDataSourcesNew()
	{
		ItemDataSourcesNew_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeRelationsImport()
	{
		ItemRelationsImport_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeSynchronize()
	{
		ItemSynchronize_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeViewData()
	{
		ItemViewData_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeCategoryNew()
	{
		ItemCategoryNew_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeVariableNew()
	{
		ItemVariableNew_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeFileNew()
	{
		ItemResourceNew_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeEdit()
	{
		ButtonEdit_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeProperties()
	{
		ItemProperties_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeDelete()
	{
		ButtonDelete_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeDictNew()
	{
		ItemDictionaryNew_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeDictOpen()
	{
		ItemDictionaryOpen_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeDictSave()
	{
		ItemDictionarySave_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeDictMerge()
	{
		ItemDictionaryMerge_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeDictXmlMerge()
	{
		ItemDictionaryXmlMerge_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeDictXmlImport()
	{
		ItemDictionaryXmlImport_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeDictXmlExport()
	{
		ItemDictionaryXmlExport_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeMarkUsed()
	{
		ItemMarkUsed_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeRemoveUnused()
	{
		ItemRemoveUnused_Click(null, EventArgs.Empty);
	}

	public static StiDictionaryPanelService GetService()
	{
		return GetService(null);
	}

	public static StiDictionaryPanelService GetService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Panels.FirstOrDefault((StiPanelService c) => c.GetType() == typeof(StiDictionaryPanelService)) as StiDictionaryPanelService;
		}
		return designer.Services.GetServices(typeof(StiPanelService)).OfType<StiDictionaryPanelService>().FirstOrDefault();
	}

	public void BuildDictionary()
	{
		if (Report != null)
		{
			UpdateDictionaryEmptyState();
			Designer.SortItems();
			DictionaryControl.CanBuildFormats = false;
			DictionaryControl.Build(Report);
			((Control)Designer).Refresh();
		}
	}

	private void CreateToolbar()
	{
		if (StiOptions.Configuration.IsWeb || !StiOptions.Engine.FullTrust)
		{
			return;
		}
		Bar obj = new Bar
		{
			RoundCorners = false
		};
		((Control)obj).Dock = (DockStyle)1;
		obj.Stretch = true;
		obj.Style = eDotNetBarStyle.Office2007;
		toolBarMain = obj;
		toolBarMain.SuspendLayout();
		buttonActions = new ButtonItem
		{
			Visible = ShowActionsButton,
			Text = StiLocalization.Get("FormDictionaryDesigner", "Actions"),
			AutoExpandOnClick = true
		};
		if (StiOptions.Designer.IsBeginnerDesigner || StiOptions.Designer.IsBICreatorDesigner)
		{
			buttonActions.Visible = false;
		}
		buttonNew = new ButtonItem
		{
			Visible = ShowNewButton,
			AutoExpandOnClick = true
		};
		buttonEdit = new ButtonItem
		{
			Visible = ShowEditButton
		};
		buttonEdit.Click += ButtonEdit_Click;
		buttonDelete = new ButtonItem
		{
			Visible = ShowDeleteButton
		};
		buttonDelete.Click += ButtonDelete_Click;
		buttonUp = new ButtonItem
		{
			BeginGroup = (ShowUpButton && (ShowActionsButton || ShowNewButton || ShowEditButton || ShowDeleteButton)),
			Visible = ShowUpButton
		};
		buttonUp.Click += ButtonUp_Click;
		buttonDown = new ButtonItem
		{
			BeginGroup = (ShowDownButton && !ShowUpButton && (ShowActionsButton || ShowNewButton || ShowEditButton || ShowDeleteButton)),
			Visible = ShowDownButton
		};
		buttonDown.Click += ButtonDown_Click;
		itemSortAsc = new ButtonItem
		{
			Text = StiLocalization.Get("FormBand", "Ascending"),
			Visible = ShowSortItemsButton,
			AutoCollapseOnClick = false,
			AutoCheckOnClick = true,
			Checked = (CurrentSort == StiDictionarySortOrder.Asc)
		};
		itemSortAsc.Click += ItemSortAsc_Click;
		itemSortDesc = new ButtonItem
		{
			Text = StiLocalization.Get("FormBand", "Descending"),
			Visible = ShowSortItemsButton,
			AutoCollapseOnClick = false,
			AutoCheckOnClick = true,
			Checked = (CurrentSort == StiDictionarySortOrder.Desc)
		};
		itemSortDesc.Click += ItemSortDesc_Click;
		itemSortNone = new ButtonItem
		{
			Text = StiLocalization.Get("FormBand", "NoSort"),
			Visible = ShowSortItemsButton,
			AutoCollapseOnClick = false,
			AutoCheckOnClick = true,
			Checked = (CurrentSort == StiDictionarySortOrder.None)
		};
		itemSortNone.Click += ItemSortNone_Click;
		buttonSettings = new ButtonItem
		{
			AutoExpandOnClick = true,
			BeginGroup = true,
			Image = StiReportDesignImages.Actions.Settings(),
			Visible = (ShowUseAliases || ShowCreateLabel || ShowCreateFieldOnDoubleClick || ShowSortItemsButton),
			HorizontalPadding = StiScaleUI.I3
		};
		buttonNew.Image = StiReportDesignImages.Actions.NewItem();
		buttonDelete.Image = StiReportDesignImages.Actions.Remove();
		buttonUp.Image = StiReportDesignImages.Actions.MoveUp();
		buttonDown.Image = StiReportDesignImages.Actions.MoveDown();
		buttonEdit.Image = StiReportDesignImages.Actions.EditData();
		itemSortAsc.Image = StiReportDesignImages.Actions.SortAZ();
		itemSortDesc.Image = StiReportDesignImages.Actions.SortZA();
		buttonSettings.Image = StiReportDesignImages.Actions.Settings();
		if (ShowCreateFieldOnDoubleClick || ShowCreateLabel || ShowUseAliases || ShowSortItemsButton)
		{
			labelCommon = new LabelItem
			{
				Height = (int)Math.Ceiling(24.0 * StiScale.Y),
				ForeColor = Color.FromArgb(255, 136, 136, 136),
				BackColor = Color.FromArgb(255, 238, 238, 238),
				Text = StiLocalization.Get("Chart", "Common")
			};
			labelSort = new LabelItem
			{
				Height = (int)Math.Ceiling(24.0 * StiScale.Y),
				ForeColor = Color.FromArgb(255, 136, 136, 136),
				BackColor = Color.FromArgb(255, 238, 238, 238),
				Text = StiLocalization.Get("PropertyMain", "Sort")
			};
			itemUseAliases = new ButtonItem
			{
				AutoCheckOnClick = true,
				AutoCollapseOnClick = false,
				GlobalItem = false,
				ThemeAware = true,
				Text = StiLocalization.Get("PropertyMain", "UseAliases")
			};
			itemUseAliases.CheckedChanged += ItemOption_CheckedChanged;
			itemUseAliases.Checked = Designer.UseAliases;
			itemCreateLabel = new ButtonItem
			{
				AutoCheckOnClick = true,
				AutoCollapseOnClick = false,
				GlobalItem = false,
				ThemeAware = true,
				Text = StiLocalization.Get("PropertyMain", "CreateLabel")
			};
			itemCreateLabel.CheckedChanged += ItemOption_CheckedChanged;
			itemCreateLabel.Checked = Designer.CreateLabel;
			itemCreateField = new ButtonItem
			{
				AutoCheckOnClick = true,
				AutoCollapseOnClick = false,
				GlobalItem = false,
				ThemeAware = true,
				Text = StiLocalization.Get("PropertyMain", "CreateFieldOnDoubleClick")
			};
			itemCreateField.CheckedChanged += ItemOption_CheckedChanged;
			itemCreateField.Checked = Designer.CreateFieldOnDoubleClick;
			if (ShowUseAliases)
			{
				buttonSettings.SubItems.Add(labelCommon);
				buttonSettings.SubItems.Add(itemUseAliases);
			}
			if (ShowCreateLabel)
			{
				buttonSettings.SubItems.Add(itemCreateLabel);
			}
			if (ShowCreateFieldOnDoubleClick)
			{
				buttonSettings.SubItems.Add(itemCreateField);
			}
			buttonSettings.SubItems.Add(labelSort);
			buttonSettings.SubItems.Add(itemSortAsc);
			buttonSettings.SubItems.Add(itemSortDesc);
			buttonSettings.SubItems.Add(itemSortNone);
		}
		if (toolBarMain != null)
		{
			toolBarMain.Items.AddRange(new BaseItem[7] { buttonActions, buttonNew, buttonEdit, buttonDelete, buttonUp, buttonDown, buttonSettings });
		}
		if (!ShowActionsButton && !ShowNewButton && !ShowEditButton && !ShowDeleteButton && !ShowUpButton && !ShowDownButton && !ShowSortItemsButton && toolBarMain != null)
		{
			toolBarMain.Visible = false;
		}
		if (toolBarMain != null)
		{
			toolBarMain.ResumeLayout();
		}
	}

	private void CreateActionsMenu()
	{
		if (!StiOptions.Configuration.IsWeb && StiOptions.Engine.FullTrust)
		{
			itemDictionaryNew = new ButtonItem(StiLocalization.Get("FormDictionaryDesigner", "DictionaryNew"));
			itemDictionaryNew.Click += ItemDictionaryNew_Click;
			itemDictionaryNew.Visible = ShowDictNewMenuItem;
			itemDictionaryOpen = new ButtonItem(StiLocalization.Get("FormDictionaryDesigner", "DictionaryOpen"));
			itemDictionaryOpen.Click += ItemDictionaryOpen_Click;
			itemDictionaryOpen.Visible = ShowDictOpenMenuItem;
			itemDictionaryMerge = new ButtonItem(StiLocalization.Get("FormDictionaryDesigner", "DictionaryMerge"));
			itemDictionaryMerge.Click += ItemDictionaryMerge_Click;
			itemDictionaryMerge.Visible = ShowDictMergeMenuItem;
			itemDictionarySave = new ButtonItem(StiLocalization.Get("FormDictionaryDesigner", "DictionarySaveAs"));
			itemDictionarySave.Click += ItemDictionarySave_Click;
			itemDictionarySave.Visible = ShowDictSaveMenuItem;
			itemDictionarySave.BeginGroup = true;
			itemDictionaryXmlImport = new ButtonItem(StiLocalization.Get("MainMenu", "menuFileImportXMLSchema"));
			itemDictionaryXmlImport.Click += ItemDictionaryXmlImport_Click;
			itemDictionaryXmlImport.Visible = ShowDictXmlImportMenuItem;
			itemDictionaryXmlImport.BeginGroup = true;
			itemDictionaryXmlExport = new ButtonItem(StiLocalization.Get("MainMenu", "menuFileExportXMLSchema"));
			itemDictionaryXmlExport.Click += ItemDictionaryXmlExport_Click;
			itemDictionaryXmlExport.Visible = ShowDictXmlExportMenuItem;
			itemDictionaryXmlMerge = new ButtonItem(StiLocalization.Get("MainMenu", "menuFileMergeXMLSchema"));
			itemDictionaryXmlMerge.Click += ItemDictionaryXmlMerge_Click;
			itemDictionaryXmlMerge.Visible = ShowDictXmlMergeMenuItem;
			itemDataSourcesNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditDataSourcesNew"));
			itemDataSourcesNew.Click += ItemDataSourcesNew_Click;
			itemDataSourcesNew.Visible = ShowDataSourcesNewMenuItem;
			itemDataSourcesNew.BeginGroup = true;
			itemRelationsImport = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditImportRelations"));
			itemRelationsImport.Click += ItemRelationsImport_Click;
			itemRelationsImport.Visible = ShowRelationsImportMenuItem;
			itemSynchronize = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditSynchronize"));
			itemSynchronize.Click += ItemSynchronize_Click;
			itemSynchronize.Visible = ShowSynchronizeMenuItem;
			itemSynchronize.BeginGroup = true;
			itemMarkUsed = new ButtonItem(StiLocalization.Get("FormDictionaryDesigner", "MarkUsedItems"));
			itemMarkUsed.Click += ItemMarkUsed_Click;
			itemMarkUsed.Visible = ShowMarkUsedMenuItem;
			itemMarkUsed.BeginGroup = true;
			itemRemoveUnused = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditRemoveUnused"));
			itemRemoveUnused.Click += ItemRemoveUnused_Click;
			itemRemoveUnused.Visible = ShowRemoveUnusedMenuItem;
			itemEmbedAllData = new ButtonItem("Save Data Snapshot");
			itemEmbedAllData.Click += ItemEmbedAllData_Click;
			itemEmbedAllData.Visible = StiAccountSettings.CurrentAccount != null;
			itemDictionaryNew.Image = StiReportDesignImages.Actions.NewFile();
			itemDictionaryOpen.Image = StiReportDesignImages.Actions.OpenFile();
			itemDictionarySave.Image = StiReportDesignImages.Actions.SaveFile();
			itemDictionaryMerge.Image = StiReportDesignImages.Actions.MergeDictionary();
			itemDictionaryXmlImport.Image = StiReportDesignImages.Actions.ImportXml();
			itemDictionaryXmlExport.Image = StiReportDesignImages.Actions.ExportXml();
			itemDictionaryXmlMerge.Image = StiReportDesignImages.Actions.MergeXml();
			itemRelationsImport.Image = StiReportDesignImages.Actions.ImportRelation();
			itemDataSourcesNew.Image = StiReportDesignImages.Actions.NewDataConnection();
			itemSynchronize.Image = StiReportDesignImages.Actions.Synchronize();
			itemMarkUsed.Image = StiReportDesignImages.Actions.MarkUsedItems();
			itemRemoveUnused.Image = StiReportDesignImages.Actions.RemoveUnused();
			itemEmbedAllData.Image = StiReportDesignImages.Actions.EmbedAllData();
			if (buttonActions != null)
			{
				buttonActions.SubItems.AddRange(new BaseItem[13]
				{
					itemDictionaryNew, itemDictionaryOpen, itemDictionaryMerge, itemDictionarySave, itemDictionaryXmlImport, itemDictionaryXmlExport, itemDictionaryXmlMerge, itemDataSourcesNew, itemRelationsImport, itemSynchronize,
					itemEmbedAllData, itemMarkUsed, itemRemoveUnused
				});
				buttonActions.PopupOpen += ButtonActionsMenu_PopupOpen;
			}
		}
	}

	private void CreateNewMenu()
	{
		itemConnectionNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditDataSourceNew"));
		itemConnectionNew.Click += ItemConnectionNew_Click;
		itemConnectionNew.Visible = ShowConnectionNewMenuItem;
		itemDataTransformationNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditDataTransformationNew"));
		itemDataTransformationNew.Click += ItemDataTransformationNew_Click;
		itemDataTransformationNew.Visible = ShowDataTransformationNewMenuItem;
		itemConnectionNewFromResource = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditDataSourceNew"));
		itemConnectionNewFromResource.GlobalItem = false;
		itemConnectionNewFromResource.Click += ItemConnectionNewFromResource_Click;
		itemBusinessObjectNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditBusinessObjectNew"));
		itemBusinessObjectNew.BeginGroup = true;
		itemBusinessObjectNew.Click += ItemBusinessObjectNew_Click;
		itemBusinessObjectNew.Visible = ShowBusinessObjectNewMenuItem;
		itemColumnNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditColumnNew"));
		itemColumnNew.Click += ItemColumnNew_Click;
		itemColumnNew.Visible = ShowColumnNewMenuItem;
		itemCalcColumnNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditCalcColumnNew"));
		itemCalcColumnNew.Click += ItemCalcColumnNew_Click;
		itemCalcColumnNew.Visible = ShowCalcColumnNewMenuItem;
		itemDataParameterNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditDataParameterNew"));
		itemDataParameterNew.Click += ItemDataParameterNew_Click;
		itemDataParameterNew.Visible = ShowDataParameterNewMenuItem;
		itemRelationNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditRelationNew"));
		itemRelationNew.Click += ItemRelationNew_Click;
		itemRelationNew.Visible = ShowRelationNewMenuItem;
		itemCategoryNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditCategoryNew"));
		itemCategoryNew.Click += ItemCategoryNew_Click;
		itemCategoryNew.Visible = ShowCategoryNewMenuItem;
		itemCategoryNew.BeginGroup = true;
		itemVariableNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditVariableNew"));
		itemVariableNew.Click += ItemVariableNew_Click;
		itemVariableNew.Visible = ShowVariableNewMenuItem;
		itemResourceNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditResourceNew"));
		itemResourceNew.Click += ItemResourceNew_Click;
		itemResourceNew.Visible = ShowResourceNewMenuItem;
		itemResourceNew.BeginGroup = true;
		itemDataTransformationNew.Image = StiReportDesignImages.Actions.NewDataTransformation();
		itemBusinessObjectNew.Image = StiReportDesignImages.Actions.NewBusinessObject();
		itemCategoryNew.Image = StiReportDesignImages.Actions.NewCategory();
		itemVariableNew.Image = StiReportDesignImages.Actions.NewVariable();
		itemResourceNew.Image = StiReportDesignImages.Actions.NewResource();
		itemConnectionNew.Image = StiReportDesignImages.Actions.NewDataConnection();
		itemColumnNew.Image = StiReportDesignImages.Actions.NewDataColumn();
		itemCalcColumnNew.Image = StiReportDesignImages.Actions.NewCalculatedColumn();
		itemRelationNew.Image = StiReportDesignImages.Actions.NewDataRelation();
		itemDataParameterNew.Image = StiReportDesignImages.Actions.NewDataParameter();
		if (buttonNew != null)
		{
			buttonNew.SubItems.AddRange(new BaseItem[11]
			{
				itemConnectionNew, itemConnectionNewFromResource, itemDataTransformationNew, itemBusinessObjectNew, itemColumnNew, itemCalcColumnNew, itemDataParameterNew, itemRelationNew, itemCategoryNew, itemVariableNew,
				itemResourceNew
			});
			buttonNew.PopupOpen += ButtonNewMenu_PopupOpen;
		}
	}

	private void CreateEditMenu()
	{
		contextItemEditMenu = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditConnectionNew"));
		contextItemEditMenu.AutoExpandOnClick = true;
		contextItemConnectionNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditDataSourceNew"));
		contextItemConnectionNew.Click += ItemConnectionNew_Click;
		contextItemConnectionNew.Visible = ShowConnectionNewMenuItem;
		contextItemConnectionNewFromResource = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditDataSourceNew"));
		contextItemConnectionNewFromResource.Click += ItemConnectionNewFromResource_Click;
		contextItemConnectionNewFromResource.GlobalItem = false;
		contextItemConnectionNewFromResource.Visible = true;
		contextItemDataTransformationNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditDataTransformationNew"));
		contextItemDataTransformationNew.Click += ItemDataTransformationNew_Click;
		contextItemDataTransformationNew.Visible = ShowDataTransformationNewMenuItem;
		contextItemBusinessObjectNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditBusinessObjectNew"));
		contextItemBusinessObjectNew.Click += ItemBusinessObjectNew_Click;
		contextItemBusinessObjectNew.Visible = ShowBusinessObjectNewMenuItem;
		contextItemBusinessObjectNew.BeginGroup = true;
		contextItemColumnNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditColumnNew"));
		contextItemColumnNew.Click += ItemColumnNew_Click;
		contextItemColumnNew.Visible = ShowColumnNewMenuItem;
		contextItemCalcColumnNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditCalcColumnNew"));
		contextItemCalcColumnNew.Click += ItemCalcColumnNew_Click;
		contextItemCalcColumnNew.Visible = ShowCalcColumnNewMenuItem;
		contextItemDataParameterNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditDataParameterNew"));
		contextItemDataParameterNew.Click += ItemDataParameterNew_Click;
		contextItemDataParameterNew.Visible = ShowDataParameterNewMenuItem;
		contextItemRelationNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditRelationNew"));
		contextItemRelationNew.Click += ItemRelationNew_Click;
		contextItemRelationNew.Visible = ShowRelationNewMenuItem;
		contextItemCategoryNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditCategoryNew"));
		contextItemCategoryNew.Click += ItemCategoryNew_Click;
		contextItemCategoryNew.Visible = ShowCategoryNewMenuItem;
		contextItemCategoryNew.BeginGroup = true;
		contextItemVariableNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditVariableNew"));
		contextItemVariableNew.Click += ItemVariableNew_Click;
		contextItemVariableNew.Visible = ShowVariableNewMenuItem;
		contextItemResourceNew = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditResourceNew"));
		contextItemResourceNew.Click += ItemResourceNew_Click;
		contextItemResourceNew.Visible = ShowResourceNewMenuItem;
		contextItemViewData = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditViewData"));
		contextItemViewData.Click += ItemViewData_Click;
		contextItemViewData.Visible = ShowViewDataMenuItem;
		contextItemViewData.BeginGroup = true;
		contextItemEdit = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditEdit"));
		contextItemEdit.Click += ButtonEdit_Click;
		contextItemEdit.Visible = ShowEditButton;
		contextItemEdit.BeginGroup = true;
		contextItemMakeThisRelationActive = new ButtonItem(StiLocalization.Get("MainMenu", "menuMakeThisRelationActive"));
		contextItemMakeThisRelationActive.Click += ItemMakeThisRelationActive_Click;
		contextItemMakeThisRelationActive.Visible = ShowMakeThisRelationActive;
		contextItemMakeThisRelationActive.BeginGroup = true;
		contextItemSaveResource = new ButtonItem(StiLocalization.Get("Buttons", "Save"));
		contextItemSaveResource.Click += ItemSaveResource_Click;
		contextItemSaveResource.Visible = ShowSaveResource;
		contextItemProperties = new ButtonItem(StiLocalization.Get("Panels", "Properties"));
		contextItemProperties.Click += ItemProperties_Click;
		contextItemProperties.Visible = ShowPropertiesMenuItem;
		contextItemDuplicate = new ButtonItem(StiLocalization.Get("Buttons", "Duplicate"));
		contextItemDuplicate.Click += ItemDuplicate_Click;
		contextItemDuplicate.Visible = ShowDuplicateMenuItem;
		contextItemDelete = new ButtonItem(StiLocalization.Get("MainMenu", "menuEditDelete"));
		contextItemDelete.Click += ButtonDelete_Click;
		contextItemDelete.Visible = ShowDeleteButton;
		contextItemExpandAll = new ButtonItem(StiLocalization.Get("Report", "ExpandAll"));
		contextItemExpandAll.Click += ItemExpandAll_Click;
		contextItemExpandAll.Visible = ShowExpandAllMenuItem;
		contextItemExpandAll.BeginGroup = true;
		contextItemCollapseAll = new ButtonItem(StiLocalization.Get("Report", "CollapseAll"));
		contextItemCollapseAll.Click += ItemCollapseAll_Click;
		contextItemCollapseAll.Visible = ShowCollapseAllMenuItem;
		contextItemDataTransformationNew.Image = StiReportDesignImages.Actions.NewDataTransformation();
		contextItemBusinessObjectNew.Image = StiReportDesignImages.Actions.NewBusinessObject();
		contextItemCategoryNew.Image = StiReportDesignImages.Actions.NewCategory();
		contextItemVariableNew.Image = StiReportDesignImages.Actions.NewVariable();
		contextItemResourceNew.Image = StiReportDesignImages.Actions.NewResource();
		contextItemConnectionNew.Image = StiReportDesignImages.Actions.NewDataConnection();
		contextItemCalcColumnNew.Image = StiReportDesignImages.Actions.NewCalculatedColumn();
		contextItemColumnNew.Image = StiReportDesignImages.Actions.NewDataColumn();
		contextItemRelationNew.Image = StiReportDesignImages.Actions.NewDataRelation();
		contextItemDelete.Image = StiReportDesignImages.Actions.Remove();
		contextItemProperties.Image = StiReportDesignImages.Actions.Properties();
		contextItemDuplicate.Image = StiReportDesignImages.Actions.Duplicate();
		contextItemEdit.Image = StiReportDesignImages.Actions.EditData();
		contextItemExpandAll.Image = StiReportDesignImages.Actions.ExpandAll();
		contextItemCollapseAll.Image = StiReportDesignImages.Actions.CollapseAll();
		contextItemViewData.Image = StiReportDesignImages.Actions.ViewData();
		contextItemDataParameterNew.Image = StiReportDesignImages.Actions.NewDataParameter();
		contextItemSaveResource.Image = StiReportDesignImages.Actions.SaveFile();
		contextItemMakeThisRelationActive.Image = StiReportDesignImages.Actions.MakeRelationActive();
		Designer.ContextMenuBar.Items.Add(contextItemEditMenu);
		if (contextItemEditMenu != null)
		{
			contextItemEditMenu.SubItems.AddRange(new BaseItem[20]
			{
				contextItemConnectionNew, contextItemConnectionNewFromResource, contextItemDataTransformationNew, contextItemBusinessObjectNew, contextItemColumnNew, contextItemCalcColumnNew, contextItemDataParameterNew, contextItemRelationNew, contextItemCategoryNew, contextItemVariableNew,
				contextItemResourceNew, contextItemViewData, contextItemMakeThisRelationActive, contextItemEdit, contextItemDuplicate, contextItemSaveResource, contextItemProperties, contextItemDelete, contextItemExpandAll, contextItemCollapseAll
			});
			contextItemEditMenu.PopupOpen += ButtonEditMenu_PopupOpen;
		}
	}

	private void UpdateDictionaryEmptyState()
	{
		if (DictionaryControl == null)
		{
			return;
		}
		if (IsEmptyDictionary && Report != null)
		{
			if (ShowConnectionNewMenuItem && isShowMore)
			{
				((Control)controlNoData).Visible = true;
				((Control)controlNoData.ButtonShowMore).Visible = true;
				((Control)DictionaryControl.tvData).Visible = false;
			}
			else
			{
				((Control)controlNoData).Visible = false;
				((Control)controlNoData.ButtonShowMore).Visible = false;
				((Control)DictionaryControl.tvData).Visible = true;
			}
		}
		else
		{
			((Control)controlNoData).Visible = false;
			((Control)controlNoData.ButtonShowMore).Visible = false;
			((Control)DictionaryControl.tvData).Visible = true;
		}
	}

	public override void Localize()
	{
		base.Localize();
		if (buttonActions != null)
		{
			if (StiOptions.Designer.ShowSuperToolTip)
			{
				string url = "user-manual/reports_designer_panels_dictionary.htm";
				Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
				Designer.SetToolTipOnlyHeaderEx(buttonActions, globalMeasureGraphics, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "Actions")), "user-manual/data_data_dictionary_control_panel_menu_actions.htm");
				Designer.SetToolTipOnlyHeaderEx(buttonNew, globalMeasureGraphics, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "NewItem")), "user-manual/data_data_dictionary_control_panel_menu_newitem.htm");
				Designer.SetToolTipOnlyHeaderEx(buttonEdit, globalMeasureGraphics, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditEdit")), url);
				Designer.SetToolTipOnlyHeaderEx(buttonDelete, globalMeasureGraphics, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditDelete")), url);
				Designer.SetToolTipOnlyHeaderEx(buttonUp, globalMeasureGraphics, StiHint.CreateHint(StiLocalization.Get("Buttons", "Up")), url);
				Designer.SetToolTipOnlyHeaderEx(buttonDown, globalMeasureGraphics, StiHint.CreateHint(StiLocalization.Get("Buttons", "Down")), url);
			}
			else
			{
				buttonActions.Tooltip = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "Actions"));
				buttonNew.Tooltip = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "NewItem"));
				buttonEdit.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditEdit"));
				buttonDelete.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditDelete"));
				buttonUp.Tooltip = StiHint.CreateHint(StiLocalization.Get("Buttons", "Up"));
				buttonDown.Tooltip = StiHint.CreateHint(StiLocalization.Get("Buttons", "Down"));
			}
			buttonSettings.Tooltip = StiHint.CreateHint(StiLocalization.Get("Export", "Settings"));
			buttonActions.Text = StiLocalization.Get("FormDictionaryDesigner", "Actions");
			itemDictionaryNew.Text = StiLocalization.Get("FormDictionaryDesigner", "DictionaryNew");
			itemDictionaryOpen.Text = StiLocalization.Get("FormDictionaryDesigner", "DictionaryOpen");
			itemDictionarySave.Text = StiLocalization.Get("FormDictionaryDesigner", "DictionarySaveAs");
			itemDictionaryMerge.Text = StiLocalization.Get("FormDictionaryDesigner", "DictionaryMerge");
			itemDictionaryXmlMerge.Text = StiLocalization.Get("MainMenu", "menuFileMergeXMLSchema");
			itemDictionaryXmlExport.Text = StiLocalization.Get("MainMenu", "menuFileExportXMLSchema");
			itemDictionaryXmlImport.Text = StiLocalization.Get("MainMenu", "menuFileImportXMLSchema");
			itemConnectionNew.Text = StiLocalization.Get("MainMenu", "menuEditDataSourceNew");
			itemDataTransformationNew.Text = StiLocalization.Get("MainMenu", "menuEditDataTransformationNew");
			itemBusinessObjectNew.Text = StiLocalization.Get("MainMenu", "menuEditBusinessObjectNew");
			itemColumnNew.Text = StiLocalization.Get("MainMenu", "menuEditColumnNew");
			itemCalcColumnNew.Text = StiLocalization.Get("MainMenu", "menuEditCalcColumnNew");
			itemDataParameterNew.Text = StiLocalization.Get("MainMenu", "menuEditDataParameterNew");
			itemRelationNew.Text = StiLocalization.Get("MainMenu", "menuEditRelationNew");
			itemCategoryNew.Text = StiLocalization.Get("MainMenu", "menuEditCategoryNew");
			itemVariableNew.Text = StiLocalization.Get("MainMenu", "menuEditVariableNew");
			itemResourceNew.Text = StiLocalization.Get("MainMenu", "menuEditResourceNew");
			itemDataSourcesNew.Text = StiLocalization.Get("MainMenu", "menuEditDataSourcesNew");
			itemRelationsImport.Text = StiLocalization.Get("MainMenu", "menuEditImportRelations");
			itemSynchronize.Text = StiLocalization.Get("MainMenu", "menuEditSynchronize");
			itemEmbedAllData.Text = StiLocalization.Get("MainMenu", "menuEmbedAllDataToResources");
			contextItemViewData.Text = StiLocalization.Get("MainMenu", "menuEditViewData");
			contextItemConnectionNew.Text = StiLocalization.Get("MainMenu", "menuEditDataSourceNew");
			contextItemDataTransformationNew.Text = StiLocalization.Get("MainMenu", "menuEditDataTransformationNew");
			contextItemBusinessObjectNew.Text = StiLocalization.Get("MainMenu", "menuEditBusinessObjectNew");
			contextItemColumnNew.Text = StiLocalization.Get("MainMenu", "menuEditColumnNew");
			contextItemCalcColumnNew.Text = StiLocalization.Get("MainMenu", "menuEditCalcColumnNew");
			contextItemDataParameterNew.Text = StiLocalization.Get("MainMenu", "menuEditDataParameterNew");
			contextItemMakeThisRelationActive.Text = StiLocalization.Get("MainMenu", "menuMakeThisRelationActive");
			contextItemRelationNew.Text = StiLocalization.Get("MainMenu", "menuEditRelationNew");
			contextItemCategoryNew.Text = StiLocalization.Get("MainMenu", "menuEditCategoryNew");
			contextItemVariableNew.Text = StiLocalization.Get("MainMenu", "menuEditVariableNew");
			contextItemResourceNew.Text = StiLocalization.Get("MainMenu", "menuEditResourceNew");
			contextItemEdit.Text = StiLocalization.Get("MainMenu", "menuEditEdit");
			contextItemSaveResource.Text = StiLocalization.Get("Buttons", "Save");
			contextItemProperties.Text = StiLocalization.Get("Panels", "Properties");
			contextItemDuplicate.Text = StiLocalization.Get("Buttons", "Duplicate");
			contextItemDelete.Text = StiLocalization.Get("MainMenu", "menuEditDelete");
			itemMarkUsed.Text = StiLocalization.Get("FormDictionaryDesigner", "MarkUsedItems");
			itemRemoveUnused.Text = StiLocalization.Get("MainMenu", "menuEditRemoveUnused");
			contextItemExpandAll.Text = StiLocalization.Get("Report", "ExpandAll");
			contextItemCollapseAll.Text = StiLocalization.Get("Report", "CollapseAll");
			itemCreateField.Text = StiLocalization.Get("PropertyMain", "CreateFieldOnDoubleClick");
			itemCreateLabel.Text = StiLocalization.Get("PropertyMain", "CreateLabel");
			itemUseAliases.Text = StiLocalization.Get("PropertyMain", "UseAliases");
			itemSortAsc.Text = StiLocalization.Get("FormBand", "Ascending");
			itemSortDesc.Text = StiLocalization.Get("FormBand", "Descending");
			itemSortNone.Text = StiLocalization.Get("FormBand", "NoSort");
			labelSort.Text = StiLocalization.Get("PropertyMain", "Sort");
			labelCommon.Text = StiLocalization.Get("Chart", "Common");
			if (controlDropFiles != null)
			{
				controlDropFiles.Localize();
			}
			if (controlNoData != null)
			{
				controlNoData.Localize();
			}
			if (DictionaryControl != null)
			{
				DictionaryControl.Build(Report);
			}
		}
	}

	public override void ApplyStyle()
	{
	}

	public override void Init(StiDesignerControl designer)
	{
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Expected O, but got Unknown
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		if (StiAccountSettings.CurrentAccount != null)
		{
			StiAccountSettings.CurrentAccount.DesignerSpecificationChanged += User_DesignerSpecificationChanged;
		}
		base.Init(designer);
		CurrentSort = (StiDictionarySortOrder)StiSettings.GetInt("Designer", "DictSortType", 2);
		if (Report != null)
		{
			switch (CurrentSort)
			{
			case StiDictionarySortOrder.Asc:
				Report.Dictionary.SortItems(StiSortOrder.Asc);
				break;
			case StiDictionarySortOrder.Desc:
				Report.Dictionary.SortItems(StiSortOrder.Desc);
				break;
			}
		}
		CreateToolbar();
		CreateActionsMenu();
		CreateNewMenu();
		CreateEditMenu();
		base.DockingItem.Image = StiReportDesignImages.Actions.Dictionary();
		StiDictionaryControl obj = new StiDictionaryControl
		{
			NodesMemory = designer.NodesMemory,
			CanBuildDatabase = true,
			CanBuildParameters = true,
			CanDragDrop = true
		};
		((Control)obj).Dock = (DockStyle)5;
		obj.Designer = designer;
		DictionaryControl = obj;
		((Control)DictionaryControl.tvData).DoubleClick += TreeViewData_DoubleClick;
		((TreeView)DictionaryControl.tvData).AfterSelect += new TreeViewEventHandler(TreeViewData_AfterSelect);
		((Control)DictionaryControl.tvData).KeyDown += new KeyEventHandler(TreeViewData_KeyDown);
		StiNoDataControl stiNoDataControl = new StiNoDataControl(this);
		((Control)stiNoDataControl).Anchor = (AnchorStyles)12;
		controlNoData = stiNoDataControl;
		panelContent = new UserControl
		{
			BackColor = StiUX.Background,
			Size = new Size((int)Math.Ceiling(250.0 * StiScale.X), (int)Math.Ceiling(250.0 * StiScale.Y)),
			Dock = (DockStyle)5
		};
		StiDictionaryDropFilesControl stiDictionaryDropFilesControl = new StiDictionaryDropFilesControl();
		((Control)stiDictionaryDropFilesControl).Size = new Size(((Control)panelContent).Width - 4, ((Control)panelContent).Height - 4);
		((Control)stiDictionaryDropFilesControl).Location = new Point(2, 2);
		((Control)stiDictionaryDropFilesControl).Dock = (DockStyle)5;
		((Control)stiDictionaryDropFilesControl).Visible = false;
		controlDropFiles = stiDictionaryDropFilesControl;
		((Control)panelContent).AllowDrop = StiOptions.Designer.AllowUseDragDrop;
		((Control)panelContent).DragEnter += new DragEventHandler(TreeViewData_DragEnter);
		((Control)panelContent).DragLeave += PanelContent_DragLeave;
		((Control)panelContent).DragOver += new DragEventHandler(TreeViewData_DragOver);
		((Control)panelContent).DragDrop += new DragEventHandler(TreeViewData_DragDropAsync);
		Designer.ContextMenuBar.SetContextMenuEx((Control)(object)DictionaryControl.tvData, contextItemEditMenu);
		Designer.ContextMenuBar.SetContextMenuEx((Control)(object)panelContent, contextItemEditMenu);
		Designer.ContextMenuBar.SetContextMenuEx((Control)(object)controlNoData, contextItemEditMenu);
		DictionaryCheck();
		designer.RefreshDictionary += Designer_RefreshDictionary;
		((Control)controlNoData.ButtonShowMore).Click += ButtonShowMore_Click;
		((Control)panelContent).Controls.Add((Control)(object)controlNoData);
		((Control)panelContent).Controls.Add((Control)(object)controlDropFiles);
		((Control)panelContent).Controls.Add((Control)(object)controlNoData.ButtonShowMore);
		((Control)panelContent).Controls.Add((Control)(object)DictionaryControl);
		UpdateDictionaryEmptyState();
		((Control)base.DockingContainer).Controls.Add((Control)(object)panelContent);
		((Control)base.DockingContainer).Controls.Add((Control)(object)toolBarMain);
	}

	public override void Dispose()
	{
		base.Dispose();
		DisposeProgress();
		DisposeComponent((Component)(object)toolBarMain);
		toolBarMain = null;
		buttonActions = null;
		buttonNew = null;
		buttonEdit = null;
		buttonDelete = null;
		buttonUp = null;
		buttonDown = null;
		itemSortAsc = null;
		itemSortDesc = null;
		itemSortNone = null;
		itemDictionaryXmlMerge = null;
		itemDictionaryXmlExport = null;
		itemDictionaryXmlImport = null;
		itemDictionarySave = null;
		itemDictionaryMerge = null;
		itemDictionaryOpen = null;
		itemDictionaryNew = null;
		itemDataSourcesNew = null;
		itemRelationsImport = null;
		itemSynchronize = null;
		itemRemoveUnused = null;
		itemMarkUsed = null;
		itemConnectionNew = null;
		itemDataTransformationNew = null;
		itemColumnNew = null;
		itemRelationNew = null;
		itemCategoryNew = null;
		itemVariableNew = null;
		itemCalcColumnNew = null;
		itemDataParameterNew = null;
		contextItemEditMenu = null;
		contextItemConnectionNew = null;
		contextItemDataTransformationNew = null;
		contextItemColumnNew = null;
		contextItemMakeThisRelationActive = null;
		contextItemRelationNew = null;
		contextItemCategoryNew = null;
		contextItemVariableNew = null;
		contextItemCalcColumnNew = null;
		contextItemDataParameterNew = null;
		contextItemViewData = null;
		contextItemEdit = null;
		contextItemProperties = null;
		contextItemDuplicate = null;
		contextItemDelete = null;
		contextItemExpandAll = null;
		contextItemCollapseAll = null;
	}

	private void User_DesignerSpecificationChanged(object sender, EventArgs e)
	{
		if (buttonActions != null)
		{
			buttonActions.Visible = StiOptions.Designer.IsDeveloperDesigner;
		}
	}

	public StiDictionaryPanelService()
	{
		CurrentSort = StiDictionarySortOrder.None;
	}

	private void TreeViewData_DragOver(object sender, DragEventArgs e)
	{
		if (Designer.Report == null)
		{
			e.Effect = (DragDropEffects)0;
			return;
		}
		if (((Control)controlDropFiles).Visible)
		{
			e.Effect = (DragDropEffects)(-2147483645);
			controlDropFiles.ProcessDragOver(e);
			return;
		}
		TreeNode draggedNode = DictionaryControl.DraggedNode;
		TreeNode nodeAt = ((TreeView)DictionaryControl.tvData).GetNodeAt(((Control)DictionaryControl.tvData).PointToClient(Cursor.Position));
		if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
		{
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (array != null)
			{
				List<string> exts = StiFileDialogHelper.GetAllResourceExtensions().ToList();
				exts.Add("dct");
				if (array.Any((string file) => exts.Any(file.EndsWithInvariantIgnoreCase)))
				{
					e.Effect = (DragDropEffects)(-2147483645);
					return;
				}
			}
			e.Effect = (DragDropEffects)0;
		}
		else if (CurrentSort != StiDictionarySortOrder.None)
		{
			e.Effect = (DragDropEffects)0;
		}
		else
		{
			if (draggedNode == nodeAt)
			{
				return;
			}
			if (draggedNode != null && draggedNode.Tag is StiVariable)
			{
				if (nodeAt != null && (nodeAt.Tag is StiVariable || (nodeAt.Text == StiLocalization.Get("PropertyMain", "Variables") && ((TreeView)DictionaryControl.tvData).Nodes.Contains(nodeAt))))
				{
					((TreeView)DictionaryControl.tvData).SelectedNode = nodeAt;
				}
			}
			else if (draggedNode != null && nodeAt != null && draggedNode.Parent == nodeAt.Parent && (draggedNode.Tag is StiDataSource || draggedNode.Tag is StiDataColumn || draggedNode.Tag is StiDatabase))
			{
				((TreeView)DictionaryControl.tvData).SelectedNode = nodeAt;
			}
			else if (draggedNode != null && nodeAt != null && draggedNode.Parent != nodeAt.Parent && draggedNode.Parent != null && nodeAt.Parent != null && draggedNode.Parent.Tag is StiDataSource && (nodeAt.Parent.Tag is StiDataSource || nodeAt.Tag is StiDataSource) && draggedNode.Tag is StiCalcDataColumn)
			{
				((TreeView)DictionaryControl.tvData).SelectedNode = nodeAt;
			}
		}
	}

	private void TreeViewData_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data != null && e.Data.GetData(DataFormats.FileDrop) is string[] array && array.Length != 0)
		{
			e.Effect = (DragDropEffects)2;
			bool flag = false;
			string[] array2 = array;
			foreach (string path in array2)
			{
				string extension = Path.GetExtension(path).ToLowerInvariant();
				StiResourceType? typeFromExtension = StiResourceTypeHelper.GetTypeFromExtension(extension);
				if (typeFromExtension.HasValue && StiResourceTypeHelper.PossibleConvertToDataSource(typeFromExtension.GetValueOrDefault()))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				((Control)controlDropFiles).Visible = true;
				((Control)controlNoData).Visible = false;
				((Control)controlNoData.ButtonShowMore).Visible = false;
				((Control)DictionaryControl.tvData).Visible = false;
				return;
			}
		}
		TreeNode draggedNode = DictionaryControl.DraggedNode;
		if (draggedNode != null && draggedNode.Tag is IStiInherited && ((IStiInherited)draggedNode.Tag).Inherited)
		{
			e.Effect = (DragDropEffects)0;
		}
		else
		{
			e.Effect = (DragDropEffects)2;
		}
	}

	private async void TreeViewData_DragDropAsync(object sender, DragEventArgs e)
	{
		_ = 1;
		try
		{
			if (((Control)controlDropFiles).Visible)
			{
				await controlDropFiles.ProcessDragDropAsync(this, e);
				((Control)controlDropFiles).Visible = false;
				controlDropFiles.Reset();
				UpdateDictionaryEmptyState();
			}
			else
			{
				await ProcessDragDropAsync(e);
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception, rethrow: false, ((Control)(Designer.Report.Designer.DesignerControl as StiDesignerControl)).FindForm());
		}
	}

	private void PanelContent_DragLeave(object sender, EventArgs e)
	{
		((Control)controlDropFiles).Visible = false;
		controlDropFiles.Reset();
		UpdateDictionaryEmptyState();
	}

	internal async Task ProcessDragDropAsync(DragEventArgs e)
	{
		if (Designer.Report == null)
		{
			return;
		}
		if (e.Data.GetData(DataFormats.FileDrop) is string[] files)
		{
			string text = null;
			object obj = null;
			string[] array = files;
			foreach (string file in array)
			{
				if (file.EndsWithInvariantIgnoreCase(".dct"))
				{
					if (!File.Exists(file))
					{
						return;
					}
					try
					{
						Designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuFileOpen"));
						Designer.Report.Dictionary.Load(file);
						Designer.BuildDictionary();
						return;
					}
					catch (Exception exception)
					{
						StiExceptionProvider.Show(exception);
						return;
					}
				}
				if (file.EndsWithInvariantIgnoreCase(".dbf") || file.EndsWithInvariantIgnoreCase(".csv") || file.EndsWithInvariantIgnoreCase(".xml") || file.EndsWithInvariantIgnoreCase(".json") || file.EndsWithInvariantIgnoreCase(".wkt") || file.EndsWithInvariantIgnoreCase(".xls") || file.EndsWithInvariantIgnoreCase(".xlsx"))
				{
					obj = await LoadFileDatabaseAsync(file, files, throwException: true);
					text = "DataSources";
				}
				if (!StiFileDialogHelper.ImageExts.Any((string ext) => file.EndsWithInvariantIgnoreCase(ext)) && !StiFileDialogHelper.FontExts.Any((string ext) => file.EndsWithInvariantIgnoreCase(ext)) && !StiFileDialogHelper.ReportExts.Any((string ext) => file.EndsWithInvariantIgnoreCase(ext)) && !StiFileDialogHelper.TextExts.Any((string ext) => file.EndsWithInvariantIgnoreCase(ext)) && !StiFileDialogHelper.DocumentExts.Any((string ext) => file.EndsWithInvariantIgnoreCase(ext)) && !StiFileDialogHelper.CustomMapExts.Any((string ext) => file.EndsWithInvariantIgnoreCase(ext)))
				{
					continue;
				}
				try
				{
					StiResource stiResource = new StiResource();
					string name = (stiResource.Alias = StiNameCreation.CreateResourceName(Designer.Report, Path.GetFileNameWithoutExtension(file)));
					stiResource.Name = name;
					stiResource.Type = StiResourceTypeHelper.GetTypeFromExtension(Path.GetExtension(file)).GetValueOrDefault(StiResourceType.Image);
					stiResource.Content = File.ReadAllBytes(file);
					Designer.Report.Dictionary.Resources.Add(stiResource);
					if (stiResource.Type == StiResourceType.Map)
					{
						StiCustomMapFinder.Clear();
					}
					text = "Resources";
					obj = stiResource;
				}
				catch
				{
				}
			}
			BuildDictionary();
			if (text != null)
			{
				DictionaryControl.SelectNodeByTag(text);
				DictionaryControl.ExpandTreeNode(((TreeView)DictionaryControl.tvData).SelectedNode);
			}
			if (obj != null)
			{
				DictionaryControl.SelectNodeByTag(obj);
			}
		}
		else
		{
			TreeNode draggedNode = DictionaryControl.DraggedNode;
			TreeNode selectedNode = ((TreeView)DictionaryControl.tvData).SelectedNode;
			if (draggedNode != null && selectedNode != null && !(selectedNode.Tag is StiDataRelation))
			{
				MoveNode(draggedNode, selectedNode);
			}
		}
	}

	internal async Task<StiFileDatabase> LoadFileDatabaseAsync(string file, string[] files, bool throwException)
	{
		try
		{
			ShowProgress(Loc.Get("DesignerFx", "LoadingData"));
			StiFileDatabase database = NewDatabaseFromExtension(file);
			if (database == null)
			{
				return null;
			}
			Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "DatabaseNew"));
			string name = (database.Alias = GetNewDatabaseName(file));
			database.Name = name;
			database.PathData = file;
			bool synchronize = false;
			StiXmlDatabase stiXmlDatabase = database as StiXmlDatabase;
			if (file.EndsWithInvariantIgnoreCase(".xml") && stiXmlDatabase != null)
			{
				stiXmlDatabase.PathSchema = files.FirstOrDefault((string f) => f.ToLowerInvariant() == Path.ChangeExtension(file.ToLowerInvariant(), "xsd"));
				stiXmlDatabase.XmlType = (string.IsNullOrWhiteSpace(stiXmlDatabase.PathSchema) ? StiXmlType.Xml : StiXmlType.AdoNetXml);
				if (file.EndsWithInvariantIgnoreCase("demo.xml") && stiXmlDatabase.PathSchema != null && stiXmlDatabase.PathSchema.EndsWithInvariantIgnoreCase("demo.xsd"))
				{
					synchronize = true;
				}
			}
			Designer.Report.Dictionary.Databases.Add(database);
			await database.CreateDataSourcesAsync(Designer.Report.Dictionary);
			if (synchronize)
			{
				await Designer.Report.Dictionary.SynchronizeRelationsAsync(database.Name);
			}
			BuildDictionary();
			return database;
		}
		catch
		{
			if (throwException)
			{
				throw;
			}
		}
		finally
		{
			HideProgress();
		}
		return null;
	}

	internal static StiFileDatabase NewDatabaseFromExtension(string file)
	{
		List<StiDatabase> databases = StiOptions.Services.Databases;
		if (file.EndsWithInvariantIgnoreCase(".dbf"))
		{
			return databases.FirstOrDefault((StiDatabase a) => a.GetType() == typeof(StiDBaseDatabase)).CreateNew() as StiFileDatabase;
		}
		if (file.EndsWithInvariantIgnoreCase(".csv"))
		{
			return databases.FirstOrDefault((StiDatabase a) => a.GetType() == typeof(StiCsvDatabase)).CreateNew() as StiFileDatabase;
		}
		if (file.EndsWithInvariantIgnoreCase(".xml"))
		{
			return databases.FirstOrDefault((StiDatabase a) => a.GetType() == typeof(StiXmlDatabase)).CreateNew() as StiFileDatabase;
		}
		if (file.EndsWithInvariantIgnoreCase(".json"))
		{
			return databases.FirstOrDefault((StiDatabase a) => a.GetType() == typeof(StiJsonDatabase)).CreateNew() as StiFileDatabase;
		}
		if (file.EndsWithInvariantIgnoreCase(".wkt"))
		{
			return databases.FirstOrDefault((StiDatabase a) => a.GetType() == typeof(StiGisDatabase)).CreateNew() as StiFileDatabase;
		}
		if (file.EndsWithInvariantIgnoreCase(".xls") || file.EndsWithInvariantIgnoreCase(".xlsx"))
		{
			return databases.FirstOrDefault((StiDatabase a) => a.GetType() == typeof(StiExcelDatabase)).CreateNew() as StiFileDatabase;
		}
		return null;
	}

	private string GetNewDatabaseName(string file)
	{
		string text = Path.GetFileNameWithoutExtension(file).Normalize();
		if (StiNameCreation.IsConnectionNameExists(Designer.Report, text))
		{
			text = StiNameCreation.CreateConnectionName(Designer.Report, text);
			if (StiNameCreation.IsConnectionNameExists(Designer.Report, text))
			{
				text = StiNameCreation.CreateConnectionName(Designer.Report, text);
			}
		}
		return text;
	}

	private async void ItemConnectionNew_Click(object sender, EventArgs e)
	{
		await InvokeConnectionNewAsync();
	}

	private async void ItemConnectionNewFromResource_Click(object sender, EventArgs e)
	{
		await InvokeConnectionNewFromResouceAsync();
	}

	private void ItemDataTransformationNew_Click(object sender, EventArgs e)
	{
		InvokeDataTransformationNew();
	}

	private void ItemBusinessObjectNew_Click(object sender, EventArgs e)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Invalid comparison between Unknown and I4
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Invalid comparison between Unknown and I4
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Invalid comparison between Unknown and I4
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			StiBusinessObject stiBusinessObject = null;
			StiBusinessObjectData stiBusinessObjectData = null;
			TreeNode selectedNode = ((TreeView)DictionaryControl.tvData).SelectedNode;
			object obj = ((selectedNode != null) ? selectedNode.Tag : null);
			if (obj is StiBusinessObject)
			{
				stiBusinessObject = obj as StiBusinessObject;
			}
			else if (obj is StiDataColumn && ((StiDataColumn)obj).BusinessObject != null)
			{
				stiBusinessObject = ((StiDataColumn)obj).BusinessObject;
			}
			StiDataColumn stiDataColumn = null;
			StiSelectTypeOfBusinessObjectForm stiSelectTypeOfBusinessObjectForm = new StiSelectTypeOfBusinessObjectForm(Report, stiBusinessObject);
			DialogResult val;
			try
			{
				Designer.AddOwnedForm((Form)(object)stiSelectTypeOfBusinessObjectForm);
				val = ((Form)stiSelectTypeOfBusinessObjectForm).ShowDialog();
				if (((RadioButton)stiSelectTypeOfBusinessObjectForm.radioButtonNewBusinessObject).Checked)
				{
					stiBusinessObject = null;
					List<StiBusinessObjectData> businessObjectsData = stiSelectTypeOfBusinessObjectForm.GetBusinessObjectsData();
					if (businessObjectsData != null && businessObjectsData.Count == 1)
					{
						stiBusinessObjectData = businessObjectsData[0];
					}
					else if ((businessObjectsData == null || businessObjectsData.Count != 0) && businessObjectsData != null)
					{
						foreach (StiBusinessObjectData item in businessObjectsData)
						{
							StiBusinessObject stiBusinessObject2 = new StiBusinessObject
							{
								Name = item.Name,
								Alias = item.Alias
							};
							Report.Dictionary.BusinessObjects.Add(stiBusinessObject2);
							object businessObjectData = stiBusinessObject2.GetBusinessObjectData();
							if (businessObjectData != null)
							{
								Report.Dictionary.SynchronizeColumns(businessObjectData, stiBusinessObject2);
							}
						}
						BuildDictionary();
						val = (DialogResult)2;
					}
				}
				if ((int)val == 1 && stiBusinessObject != null)
				{
					StiDataColumnsCollection columns = stiSelectTypeOfBusinessObjectForm.GetColumns();
					if (columns.Count == 1)
					{
						stiDataColumn = columns[0];
					}
					else if (columns.Count > 1)
					{
						foreach (StiDataColumn item2 in columns)
						{
							StiBusinessObject stiBusinessObject3 = new StiBusinessObject
							{
								Name = item2.Name,
								Alias = item2.Alias
							};
							stiBusinessObject.BusinessObjects.Add(stiBusinessObject3);
							object businessObjectData2 = stiBusinessObject3.GetBusinessObjectData();
							if (businessObjectData2 != null)
							{
								Report.Dictionary.SynchronizeColumns(businessObjectData2, stiBusinessObject3);
							}
						}
						BuildDictionary();
						val = (DialogResult)2;
					}
				}
			}
			finally
			{
				((IDisposable)stiSelectTypeOfBusinessObjectForm)?.Dispose();
			}
			if ((int)val != 1)
			{
				return;
			}
			Designer.UndoRedoSave(StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditBusinessObjectNew")));
			StiBusinessObject stiBusinessObject4 = new StiBusinessObject();
			if (stiBusinessObject != null)
			{
				if (stiDataColumn != null)
				{
					stiBusinessObject4.Name = stiDataColumn.Name;
					stiBusinessObject4.Alias = stiDataColumn.Alias;
				}
				stiBusinessObject.BusinessObjects.Add(stiBusinessObject4);
				object businessObjectData3 = stiBusinessObject4.GetBusinessObjectData();
				if (businessObjectData3 != null)
				{
					Report.Dictionary.SynchronizeColumns(businessObjectData3, stiBusinessObject4);
				}
			}
			else
			{
				if (stiBusinessObjectData != null)
				{
					stiBusinessObject4.Name = stiBusinessObjectData.Name;
					stiBusinessObject4.Alias = stiBusinessObjectData.Alias;
				}
				Report.Dictionary.BusinessObjects.Add(stiBusinessObject4);
			}
			StiBusinessObjectEditForm stiBusinessObjectEditForm = new StiBusinessObjectEditForm(stiBusinessObject4, newForm: true);
			try
			{
				if (Report.Designer != null && Report.Designer.DesignerControl != null)
				{
					Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiBusinessObjectEditForm);
				}
				if ((int)((Form)stiBusinessObjectEditForm).ShowDialog() == 1)
				{
					BuildDictionary();
					DictionaryControl.SelectNodeByTag(stiBusinessObject4);
					StiCacheCleaner.Clean(Designer?.Report);
					return;
				}
				if (stiBusinessObject != null)
				{
					stiBusinessObject.BusinessObjects.Remove(stiBusinessObject4);
				}
				else
				{
					Report.Dictionary.BusinessObjects.Remove(stiBusinessObject4);
				}
				if (Designer.UndoRedo != null && Designer.UndoRedo.CanUndo)
				{
					Designer.UndoRedo.UndoStackPop();
				}
			}
			finally
			{
				((IDisposable)stiBusinessObjectEditForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemColumnNew_Click(object sender, EventArgs e)
	{
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Invalid comparison between Unknown and I4
		try
		{
			if (((TreeView)DictionaryControl.tvData).SelectedNode == null)
			{
				return;
			}
			object tag = ((TreeView)DictionaryControl.tvData).SelectedNode.Tag;
			StiDataColumnEditForm stiDataColumnEditForm = new StiDataColumnEditForm(isEditMode: false);
			try
			{
				if (Designer != null)
				{
					Designer.AddOwnedForm((Form)(object)stiDataColumnEditForm);
				}
				StiTextBox textBoxNameInSource = stiDataColumnEditForm.textBoxNameInSource;
				StiTextBox tbName = stiDataColumnEditForm.tbName;
				string text2 = (((Control)stiDataColumnEditForm.tbAlias).Text = StiLocalization.Get("PropertyMain", "Column"));
				string text4 = (((Control)tbName).Text = text2);
				((Control)textBoxNameInSource).Text = text4;
				((Control)stiDataColumnEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "ColumnNew"));
				ObjectCollection items = ((ComboBox)stiDataColumnEditForm.cbType).Items;
				object[] typeWrappers = StiTypeWrapper.GetTypeWrappers();
				items.AddRange(typeWrappers);
				foreach (StiTypeWrapper item in ((ComboBox)stiDataColumnEditForm.cbType).Items)
				{
					if (item.Type == typeof(string))
					{
						((ComboBox)stiDataColumnEditForm.cbType).SelectedItem = item;
						break;
					}
				}
				if (tag is StiDataTransformation || tag is StiDataTransformationColumn)
				{
					stiDataColumnEditForm.Expression = string.Empty;
				}
				if ((int)((Form)stiDataColumnEditForm).ShowDialog() == 1)
				{
					Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "ColumnNew"));
					StiDataColumn stiDataColumn = ((tag is StiDataTransformation || tag is StiDataTransformationColumn) ? new StiDataTransformationColumn(((Control)stiDataColumnEditForm.tbName).Text, ((Control)stiDataColumnEditForm.tbAlias).Text, ((StiTypeWrapper)((ComboBox)stiDataColumnEditForm.cbType).SelectedItem).Type, stiDataColumnEditForm.Expression)
					{
						NameInSource = ((Control)stiDataColumnEditForm.textBoxNameInSource).Text,
						Index = -1
					} : new StiDataColumn(((Control)stiDataColumnEditForm.textBoxNameInSource).Text, ((Control)stiDataColumnEditForm.tbName).Text, ((Control)stiDataColumnEditForm.tbAlias).Text, ((StiTypeWrapper)((ComboBox)stiDataColumnEditForm.cbType).SelectedItem).Type));
					if (tag is StiDataSource)
					{
						((StiDataSource)((TreeView)DictionaryControl.tvData).SelectedNode.Tag).Columns.Add(stiDataColumn);
					}
					else if (tag is StiBusinessObject)
					{
						((StiBusinessObject)((TreeView)DictionaryControl.tvData).SelectedNode.Tag).Columns.Add(stiDataColumn);
					}
					else
					{
						((StiDataColumn)((TreeView)DictionaryControl.tvData).SelectedNode.Tag).DataSource.Columns.Add(stiDataColumn);
					}
					((TreeView)DictionaryControl.tvData).SelectedNode.Nodes.Add(stiDataColumn.Name);
					((TreeView)DictionaryControl.tvData).SelectedNode.Expand();
					BuildDictionary();
					DictionaryControl.SelectNodeByTag(stiDataColumn);
					StiCacheCleaner.Clean(Designer?.Report);
				}
			}
			finally
			{
				((IDisposable)stiDataColumnEditForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemCalcColumnNew_Click(object sender, EventArgs e)
	{
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Invalid comparison between Unknown and I4
		try
		{
			object tag = ((TreeView)DictionaryControl.tvData).SelectedNode.Tag;
			StiCalcDataColumnEditForm stiCalcDataColumnEditForm = new StiCalcDataColumnEditForm(Designer.Report, isEditMode: false);
			try
			{
				if (Designer != null)
				{
					Designer.AddOwnedForm((Form)(object)stiCalcDataColumnEditForm);
				}
				StiTextBox tbName = stiCalcDataColumnEditForm.tbName;
				string text2 = (((Control)stiCalcDataColumnEditForm.tbAlias).Text = StiLocalization.Get("PropertyMain", "Column"));
				((Control)tbName).Text = text2;
				((Control)stiCalcDataColumnEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "CalcColumnNew"));
				ObjectCollection items = ((ComboBox)stiCalcDataColumnEditForm.cbType).Items;
				object[] typeWrappers = StiTypeWrapper.GetTypeWrappers();
				items.AddRange(typeWrappers);
				foreach (StiTypeWrapper item in ((ComboBox)stiCalcDataColumnEditForm.cbType).Items)
				{
					if (item.Type == typeof(string))
					{
						((ComboBox)stiCalcDataColumnEditForm.cbType).SelectedItem = item;
						break;
					}
				}
				if ((int)((Form)stiCalcDataColumnEditForm).ShowDialog() == 1)
				{
					Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "ColumnNew"));
					StiCalcDataColumn stiCalcDataColumn = new StiCalcDataColumn(((Control)stiCalcDataColumnEditForm.tbName).Text, ((Control)stiCalcDataColumnEditForm.tbAlias).Text, ((StiTypeWrapper)((ComboBox)stiCalcDataColumnEditForm.cbType).SelectedItem).Type, ((Control)stiCalcDataColumnEditForm.tbValue).Text);
					if (tag is StiDataSource)
					{
						((StiDataSource)((TreeView)DictionaryControl.tvData).SelectedNode.Tag).Columns.Add(stiCalcDataColumn);
					}
					else if (tag is StiBusinessObject)
					{
						((StiBusinessObject)((TreeView)DictionaryControl.tvData).SelectedNode.Tag).Columns.Add(stiCalcDataColumn);
					}
					else
					{
						((StiDataColumn)((TreeView)DictionaryControl.tvData).SelectedNode.Tag).DataSource.Columns.Add(stiCalcDataColumn);
					}
					((TreeView)DictionaryControl.tvData).SelectedNode.Nodes.Add(stiCalcDataColumn.Name);
					((TreeView)DictionaryControl.tvData).SelectedNode.Expand();
					BuildDictionary();
					DictionaryControl.SelectNodeByTag(stiCalcDataColumn);
					StiCacheCleaner.Clean(Designer?.Report);
				}
			}
			finally
			{
				((IDisposable)stiCalcDataColumnEditForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemDataParameterNew_Click(object sender, EventArgs e)
	{
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Invalid comparison between Unknown and I4
		try
		{
			StiSqlSource sqlSource = GetSqlSource(((TreeView)DictionaryControl.tvData).SelectedNode);
			if (sqlSource == null)
			{
				return;
			}
			StiDataParameter stiDataParameter = sqlSource.AddParameter();
			StiDataParameterEditForm stiDataParameterEditForm = new StiDataParameterEditForm(Designer.Report, isEditMode: false);
			try
			{
				if (Designer != null)
				{
					Designer.AddOwnedForm((Form)(object)stiDataParameterEditForm);
				}
				((Control)stiDataParameterEditForm.tbName).Text = StiLocalization.Get("PropertyMain", "Parameter");
				((Control)stiDataParameterEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "DataParameterNew"));
				Type parameterTypesEnum = ((StiSqlSource)stiDataParameter.DataSource).GetParameterTypesEnum();
				Array values = Enum.GetValues(parameterTypesEnum);
				object[] array = new object[values.Length];
				values.CopyTo(array, 0);
				((ComboBox)stiDataParameterEditForm.cbType).Items.AddRange(array);
				((ComboBox)stiDataParameterEditForm.cbType).Sorted = true;
				int num = 0;
				foreach (object item in ((ComboBox)stiDataParameterEditForm.cbType).Items)
				{
					if ((int)item == stiDataParameter.Type)
					{
						((ListControl)stiDataParameterEditForm.cbType).SelectedIndex = num;
						break;
					}
					num++;
				}
				if ((int)((Form)stiDataParameterEditForm).ShowDialog() == 1)
				{
					Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "DataParameterNew"));
					stiDataParameter.Name = ((Control)stiDataParameterEditForm.tbName).Text;
					stiDataParameter.Expression = ((Control)stiDataParameterEditForm.tbValue).Text;
					stiDataParameter.Type = (int)((ComboBox)stiDataParameterEditForm.cbType).SelectedItem;
					((TreeView)DictionaryControl.tvData).SelectedNode.Nodes.Add(stiDataParameter.Name);
					((TreeView)DictionaryControl.tvData).SelectedNode.Expand();
					BuildDictionary();
					DictionaryControl.SelectNodeByTag(stiDataParameter);
					StiCacheCleaner.Clean(Designer?.Report);
				}
				else
				{
					sqlSource.Parameters.Remove(stiDataParameter);
				}
			}
			finally
			{
				((IDisposable)stiDataParameterEditForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemRelationNew_Click(object sender, EventArgs e)
	{
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			StiDataSource childSource = null;
			if (((TreeView)DictionaryControl.tvData).SelectedNode != null && ((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiDataSource)
			{
				childSource = ((TreeView)DictionaryControl.tvData).SelectedNode.Tag as StiDataSource;
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode != null && ((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiDataColumn)
			{
				childSource = ((StiDataColumn)((TreeView)DictionaryControl.tvData).SelectedNode.Tag).DataSource;
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode != null && ((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiDataRelation)
			{
				childSource = ((StiDataRelation)((TreeView)DictionaryControl.tvData).SelectedNode.Tag).ChildSource;
			}
			ItemRelationNew_Click(childSource, null);
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	internal DialogResult ItemRelationNew_Click(StiDataSource childSource, StiDataSource parentSource)
	{
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Invalid comparison between Unknown and I4
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Expected O, but got Unknown
		try
		{
			StiDataRelationEditForm stiDataRelationEditForm = new StiDataRelationEditForm(isEditMode: false);
			try
			{
				Designer.AddOwnedForm((Form)(object)stiDataRelationEditForm);
				stiDataRelationEditForm.AddRelationDataSources(Designer.Report.Dictionary.DataSources.ToList());
				stiDataRelationEditForm.RelationChildDataSource = childSource;
				stiDataRelationEditForm.RelationParentDataSource = parentSource;
				string text = StiLocalization.Get("PropertyMain", "Relation");
				int num = 1;
				string text2;
				while (true)
				{
					text2 = ((num == 1) ? text : (text + num));
					bool flag = false;
					foreach (StiDataRelation relation2 in Designer.Report.Dictionary.Relations)
					{
						if (relation2.NameInSource == text2)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
					num++;
				}
				stiDataRelationEditForm.RelationNameInSource = text2;
				stiDataRelationEditForm.RelationName = StiLocalization.Get("PropertyMain", "Name");
				stiDataRelationEditForm.RelationAlias = StiLocalization.Get("PropertyMain", "Name");
				((Control)stiDataRelationEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "RelationNew"));
				if ((int)((Form)stiDataRelationEditForm).ShowDialog() == 1)
				{
					Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "RelationNew"));
					StiDataRelation relation = stiDataRelationEditForm.GetRelation();
					Designer.Report.Dictionary.Relations.Add(relation);
					((TreeView)DictionaryControl.tvData).SelectedNode.Nodes.Add(relation.Name);
					if (relation.Active)
					{
						(from r in relation.ChildSource.ParentRelationList()
							where r != relation
							select r).ToList().ForEach(delegate(StiDataRelation r)
						{
							r.Active = false;
						});
					}
					BuildDictionary();
					DictionaryControl.SelectNodeByTag(relation.ChildSource);
					((TreeView)DictionaryControl.tvData).SelectedNode.Expand();
					if (((TreeView)DictionaryControl.tvData).SelectedNode != null)
					{
						foreach (TreeNode node in ((TreeView)DictionaryControl.tvData).SelectedNode.Nodes)
						{
							TreeNode val = node;
							if (val.Tag == relation)
							{
								((TreeView)DictionaryControl.tvData).SelectedNode = val;
								break;
							}
						}
					}
					StiCacheCleaner.Clean(Designer?.Report);
					return (DialogResult)1;
				}
				return (DialogResult)2;
			}
			finally
			{
				((IDisposable)stiDataRelationEditForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
			return (DialogResult)0;
		}
	}

	private void ItemDataSourcesNew_Click(object sender, EventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		try
		{
			StiDataSourcesNewForm stiDataSourcesNewForm = new StiDataSourcesNewForm(Designer.Report.Dictionary);
			try
			{
				Designer.AddOwnedForm((Form)(object)stiDataSourcesNewForm);
				if ((int)((Form)stiDataSourcesNewForm).ShowDialog() != 1)
				{
					return;
				}
				Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "DataSourcesNew"));
				foreach (TreeNode node in ((TreeView)stiDataSourcesNewForm.dataControl.tvData).Nodes)
				{
					TreeNode val = node;
					StiDataAdapterService stiDataAdapterService = val.Tag as StiDataAdapterService;
					foreach (TreeNode node2 in val.Nodes)
					{
						TreeNode val2 = node2;
						if (val2.Checked)
						{
							StiDataSource stiDataSource = stiDataAdapterService.Create(Designer.Report.Dictionary);
							stiDataAdapterService.SetDataSourceNames(val2.Tag as StiData, stiDataSource);
							StiDataColumnsCollection columnsFromData = StiDataLeader.GetColumnsFromData(stiDataAdapterService, val2.Tag as StiData, stiDataSource);
							stiDataSource.Columns.AddRange(columnsFromData);
						}
					}
				}
				Report.Dictionary.Connect(loadData: false);
				BuildDictionary();
				StiCacheCleaner.Clean(Designer?.Report);
			}
			finally
			{
				((IDisposable)stiDataSourcesNewForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemRelationsImport_Click(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and I4
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		try
		{
			Hashtable categories = new Hashtable();
			StiDataRelationsCollection unusedRelationsFromDataStore = Designer.Report.Dictionary.GetUnusedRelationsFromDataStore(ref categories);
			if (unusedRelationsFromDataStore.Count == 0)
			{
				StiLogService.Write(GetType(), StiLocalization.Get("Errors", "RelationsNotFound"));
				MessageBox.Show(StiLocalization.Get("Errors", "RelationsNotFound"));
				return;
			}
			StiDataRelationListForm stiDataRelationListForm = new StiDataRelationListForm(categories, unusedRelationsFromDataStore);
			try
			{
				Designer.AddOwnedForm((Form)(object)stiDataRelationListForm);
				if ((int)((Form)stiDataRelationListForm).ShowDialog() != 1)
				{
					return;
				}
				Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "ImportRelations"));
				foreach (TreeNode node in ((TreeView)stiDataRelationListForm.tvRelations).Nodes)
				{
					TreeNode val = node;
					foreach (TreeNode node2 in val.Nodes)
					{
						TreeNode val2 = node2;
						if (val2.Checked)
						{
							Designer.Report.Dictionary.Relations.Add(val2.Tag as StiDataRelation);
						}
					}
				}
				BuildDictionary();
				StiCacheCleaner.Clean(Designer.Report);
			}
			finally
			{
				((IDisposable)stiDataRelationListForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemSynchronize_Click(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		try
		{
			if ((int)MessageBox.Show(StiLocalization.Get("Questions", "qnSynchronize"), StiLocalization.Get("FormDictionaryDesigner", "title"), (MessageBoxButtons)4) == 6)
			{
				Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "Synchronize"));
				Designer.Report.Dictionary.Synchronize();
				BuildDictionary();
				StiCacheCleaner.Clean(Designer.Report);
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private async void ItemEmbedAllData_Click(object sender, EventArgs e)
	{
		try
		{
			DialogResult val = MessageBox.Show(string.Format(StiLocalization.Get("Messages", "ThisFunctionEmbedsAllReportDataToTheReport"), Environment.NewLine + Environment.NewLine), "Stimulsoft Designer", (MessageBoxButtons)4, (MessageBoxIcon)48);
			if ((int)val == 6)
			{
				Designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuEmbedAllDataToResources"));
				StiDictionaryPanelHelper.ShowProgress(Designer, StiLocalization.Get("MainMenu", "menuEmbedAllDataToResources"));
				await StiDataResourceHelper.SaveSnapshotAsync(Designer.Report);
				GetService(Designer).BuildDictionary();
			}
		}
		finally
		{
			StiDictionaryPanelHelper.HideProgress(Designer);
		}
	}

	private void ItemViewData_Click(object sender, EventArgs e)
	{
		StiDataSource stiDataSource = ((TreeView)DictionaryControl.tvData).SelectedNode.Tag as StiDataSource;
		try
		{
			if (stiDataSource != null)
			{
				StiViewDataForm stiViewDataForm = new StiViewDataForm(stiDataSource);
				if (!stiViewDataForm.IsCancelSqlExpressionsForm)
				{
					if (Designer != null)
					{
						Designer.AddOwnedForm((Form)(object)stiViewDataForm);
					}
					((Control)stiViewDataForm).Show();
				}
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(GetType(), ex);
			StiExceptionProvider.Show(ex);
		}
		finally
		{
			if (stiDataSource is StiVirtualSource)
			{
				stiDataSource.Dictionary.Disconnect();
			}
		}
		StiBusinessObject stiBusinessObject = ((TreeView)DictionaryControl.tvData).SelectedNode.Tag as StiBusinessObject;
		try
		{
			if (stiBusinessObject != null)
			{
				StiViewDataForm stiViewDataForm2 = new StiViewDataForm(stiBusinessObject);
				if (Designer != null)
				{
					Designer.AddOwnedForm((Form)(object)stiViewDataForm2);
				}
				((Control)stiViewDataForm2).Show();
			}
		}
		catch (Exception ex2)
		{
			StiLogService.Write(GetType(), ex2);
			StiExceptionProvider.Show(ex2);
		}
	}

	private void ItemCategoryNew_Click(object sender, EventArgs e)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Invalid comparison between Unknown and I4
		try
		{
			Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "CategoryNew"));
			StiCategoryEditForm stiCategoryEditForm = new StiCategoryEditForm(Designer, Designer.Report.Dictionary.Variables.GenerateEmptyCategoryName(), isEditMode: false, isVariableMode: true);
			try
			{
				if (Designer != null)
				{
					Designer.AddOwnedForm((Form)(object)stiCategoryEditForm);
				}
				((Control)stiCategoryEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "CategoryNew"));
				if ((int)((Form)stiCategoryEditForm).ShowDialog() == 1)
				{
					StiVariable stiVariable = new StiVariable(stiCategoryEditForm.NewCategoryName);
					Designer.Report.Dictionary.Variables.Add(stiVariable);
					BuildDictionary();
					if (((TreeView)DictionaryControl.tvData).SelectedNode == null || !(((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiVariable))
					{
						DictionaryControl.SelectNodeByTag("Variables");
					}
					DictionaryControl.ExpandTreeNode(((TreeView)DictionaryControl.tvData).SelectedNode);
					DictionaryControl.SelectNodeByTag(stiVariable);
					return;
				}
			}
			finally
			{
				((IDisposable)stiCategoryEditForm)?.Dispose();
			}
			if (Designer.UndoRedo != null && Designer.UndoRedo.CanUndo)
			{
				Designer.UndoRedo.UndoStackPop();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemVariableNew_Click(object sender, EventArgs e)
	{
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Invalid comparison between Unknown and I4
		try
		{
			using IStiVariableEditForm stiVariableEditForm = StiDictionaryFormCreator.CreateVariableEditorForm(Designer.Report, null, isEditMode: false);
			if (Designer != null)
			{
				Designer.AddOwnedForm((Form)((stiVariableEditForm is Form) ? stiVariableEditForm : null));
			}
			stiVariableEditForm.Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "VariableNew"));
			string category = string.Empty;
			if (((TreeView)DictionaryControl.tvData).SelectedNode != null && ((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiVariable)
			{
				category = ((((TreeView)DictionaryControl.tvData).SelectedNode.Tag == null) ? ((TreeView)DictionaryControl.tvData).SelectedNode.Text : ((StiVariable)((TreeView)DictionaryControl.tvData).SelectedNode.Tag).Category);
			}
			stiVariableEditForm.VariableType = typeof(string);
			stiVariableEditForm.VariableReadOnly = false;
			stiVariableEditForm.VariableAllowUserValues = true;
			stiVariableEditForm.SetBaseName(StiNameCreation.CreateName(Report, StiLocalization.Get("PropertyMain", "Variable")));
			if ((int)stiVariableEditForm.ShowDialog() != 1)
			{
				return;
			}
			Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "VariableNew"));
			Type variableType = stiVariableEditForm.VariableType;
			StiVariable stiVariable = new StiVariable(value: (!(variableType == typeof(DateTime)) && !(variableType == typeof(TimeSpan))) ? stiVariableEditForm.VariableDefaultValue : string.Empty, category: category, name: stiVariableEditForm.VariableName.Trim(), alias: stiVariableEditForm.VariableAlias, description: stiVariableEditForm.VariableDescription, type: variableType, readOnly: stiVariableEditForm.VariableReadOnly, initBy: stiVariableEditForm.VariableInitBy, requestFromUser: stiVariableEditForm.VariableRequestFromUser)
			{
				AllowUseAsSqlParameter = stiVariableEditForm.VariableAllowUsingAsSqlParameter
			};
			if (stiVariable.InitBy == StiVariableInitBy.Expression)
			{
				if (StiTypeFinder.FindType(variableType, typeof(Range)))
				{
					stiVariable.InitByExpressionFrom = StiExpressionPacker.UnPackExpression(stiVariableEditForm.VariableInitByExpressionFrom, Designer, useBraces: false);
					stiVariable.InitByExpressionTo = StiExpressionPacker.UnPackExpression(stiVariableEditForm.VariableInitByExpressionTo, Designer, useBraces: false);
				}
				else
				{
					stiVariable.Value = StiExpressionPacker.UnPackExpression(stiVariableEditForm.VariableInitByExpression, Designer, useBraces: false);
				}
			}
			else
			{
				bool flag = variableType == typeof(DateTime) || variableType == typeof(DateTime?) || variableType == typeof(DateTimeRange);
				bool flag2 = variableType == typeof(TimeSpan) || variableType == typeof(TimeSpan?) || variableType == typeof(TimeSpanRange);
				if (flag || flag2)
				{
					if (variableType == typeof(DateTimeRange) || variableType == typeof(TimeSpanRange))
					{
						if (variableType == typeof(DateTimeRange))
						{
							DateTime? from = ((stiVariableEditForm.VariableNotAssignedFrom || stiVariableEditForm.IsVariableDefaultValueIgnored) ? null : new DateTime?(stiVariableEditForm.VariableDateTimeDefaultValueFrom));
							DateTime? to = ((stiVariableEditForm.VariableNotAssignedTo || stiVariableEditForm.IsVariableDefaultValueIgnored) ? null : new DateTime?(stiVariableEditForm.VariableDateTimeDefaultValueTo));
							stiVariable.ValueObject = new DateTimeRange(from, to);
						}
						if (variableType == typeof(TimeSpanRange))
						{
							TimeSpan? from2 = (stiVariableEditForm.VariableTimeSpanNotAssignedFrom ? null : new TimeSpan?(stiVariableEditForm.VariableTimeSpanDefaultValueFrom));
							TimeSpan? to2 = (stiVariableEditForm.VariableTimeSpanNotAssignedTo ? null : new TimeSpan?(stiVariableEditForm.VariableTimeSpanDefaultValueTo));
							stiVariable.ValueObject = new TimeSpanRange(from2, to2);
						}
					}
					else
					{
						if (flag)
						{
							if (stiVariableEditForm.VariableNotAssigned || stiVariableEditForm.IsVariableDefaultValueIgnored)
							{
								stiVariable.ValueObject = null;
							}
							else
							{
								stiVariable.ValueObject = stiVariableEditForm.VariableDateTimeDefaultValue;
							}
						}
						if (flag2)
						{
							if (stiVariableEditForm.VariableTimeSpanNotAssigned || stiVariableEditForm.IsVariableDefaultValueIgnored)
							{
								stiVariable.ValueObject = null;
							}
							else
							{
								stiVariable.ValueObject = stiVariableEditForm.VariableTimeSpanDefaultValue;
							}
						}
					}
				}
				else if (variableType == typeof(Image))
				{
					if (stiVariableEditForm.VariableImage == null)
					{
						stiVariable.Type = typeof(Image);
						stiVariable.ValueObject = null;
					}
					else
					{
						stiVariable.Type = ((object)stiVariableEditForm.VariableImage).GetType();
						stiVariable.ValueObject = stiVariableEditForm.VariableImage;
					}
				}
				else if (variableType == typeof(bool))
				{
					stiVariable.Type = typeof(bool);
					stiVariable.ValueObject = stiVariableEditForm.VariableBoolDefaultValue;
				}
				else if (variableType == typeof(bool?))
				{
					stiVariable.Type = typeof(bool?);
					stiVariable.ValueObject = stiVariableEditForm.VariableBoolDefaultValue;
				}
				else if (StiTypeFinder.FindType(variableType, typeof(Range)))
				{
					Range range = Activator.CreateInstance(variableType) as Range;
					range.Parse(stiVariableEditForm.VariableDefaultValueFrom, stiVariableEditForm.VariableDefaultValueTo);
					stiVariable.ValueObject = range;
				}
				else
				{
					stiVariable.Value = stiVariableEditForm.VariableDefaultValue;
				}
			}
			if (stiVariableEditForm.VariableRequestFromUser)
			{
				if (stiVariable.DialogInfo == null)
				{
					stiVariable.DialogInfo = new StiDialogInfo();
				}
				stiVariable.DialogInfo.AllowUserValues = stiVariableEditForm.VariableAllowUserValues;
				stiVariable.DialogInfo.BindingValue = stiVariableEditForm.VariableBindingValue;
				stiVariable.DialogInfo.ItemsInitializationType = stiVariableEditForm.VariableDataSourceInitializationType;
				stiVariable.DialogInfo.SortDirection = stiVariableEditForm.VariableSortDirection;
				stiVariable.DialogInfo.SortField = stiVariableEditForm.VariableSortField;
				if (stiVariable.DialogInfo.ItemsInitializationType == StiItemsInitializationType.Columns)
				{
					stiVariable.DialogInfo.KeysColumn = stiVariableEditForm.VariableKeysColumn;
					stiVariable.DialogInfo.ValuesColumn = stiVariableEditForm.VariableValuesColumn;
					stiVariable.DialogInfo.CheckedColumn = stiVariableEditForm.VariableCheckedColumn;
					stiVariable.DialogInfo.BindingVariable = stiVariableEditForm.VariableBindingVariable;
					stiVariable.DialogInfo.BindingValuesColumn = stiVariableEditForm.VariableBindingColumn;
					stiVariable.DialogInfo.Keys = new string[0];
					stiVariable.DialogInfo.Values = new string[0];
					stiVariable.DialogInfo.CheckedStates = new bool[0];
				}
				else
				{
					stiVariable.DialogInfo.KeysColumn = string.Empty;
					stiVariable.DialogInfo.ValuesColumn = string.Empty;
					stiVariable.DialogInfo.CheckedColumn = string.Empty;
					stiVariable.DialogInfo.BindingVariable = null;
					stiVariable.DialogInfo.BindingValuesColumn = string.Empty;
					stiVariable.DialogInfo.SetDialogInfoItems(stiVariableEditForm.Items, variableType);
				}
				if (variableType == typeof(DateTime) || variableType == typeof(DateTime?) || variableType == typeof(DateTimeRange) || variableType == typeof(DateTimeList))
				{
					stiVariable.DialogInfo.DateTimeType = stiVariableEditForm.VariableDateTimeType;
				}
				if (variableType == typeof(string) || variableType == typeof(StringRange) || variableType == typeof(StringList))
				{
					stiVariable.DialogInfo.Mask = stiVariableEditForm.VariableMask;
				}
			}
			stiVariable.Selection = stiVariableEditForm.VariableSelectionMode;
			if (StiNameCreation.Exists(Designer.Report, stiVariable.Name))
			{
				string text = StiNameCreation.CreateName(Designer.Report, stiVariable.Name);
				if (stiVariable.Name == stiVariable.Alias)
				{
					stiVariable.Alias = text;
				}
				stiVariable.Name = text;
			}
			Designer.Report.Dictionary.Variables.Add(stiVariable);
			BuildDictionary();
			if (((TreeView)DictionaryControl.tvData).SelectedNode == null || !(((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiVariable))
			{
				DictionaryControl.SelectNodeByTag("Variables");
			}
			DictionaryControl.ExpandTreeNode(((TreeView)DictionaryControl.tvData).SelectedNode);
			DictionaryControl.SelectNodeByTag(stiVariable);
			StiCacheCleaner.Clean(Designer?.Report);
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemResourceNew_Click(object sender, EventArgs e)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Invalid comparison between Unknown and I4
		try
		{
			StiResourceEditForm stiResourceEditForm = new StiResourceEditForm(Designer.Report, isEditMode: false, StiResourceType.Image, null);
			try
			{
				if (Designer != null)
				{
					Designer.AddOwnedForm((Form)(object)stiResourceEditForm);
				}
				((Control)stiResourceEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "ResourceNew"));
				stiResourceEditForm.SetBaseName(StiNameCreation.CreateResourceName(Report, StiLocalization.Get("PropertyMain", "Resource")));
				if ((int)((Form)stiResourceEditForm).ShowDialog() == 1)
				{
					StiResource stiResource = new StiResource(((Control)stiResourceEditForm.textBoxName).Text, ((Control)stiResourceEditForm.textBoxAlias).Text, stiResourceEditForm.ResourceType, stiResourceEditForm.Content, ((CheckBox)stiResourceEditForm.checkBoxAvailableInTheViewer).Checked && ((Control)stiResourceEditForm.checkBoxAvailableInTheViewer).Enabled);
					Designer.Report.Dictionary.Resources.Add(stiResource);
					if (stiResource.Type == StiResourceType.Map)
					{
						StiCustomMapFinder.Clear();
					}
					BuildDictionary();
					if (((TreeView)DictionaryControl.tvData).SelectedNode == null || !(((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiResource))
					{
						DictionaryControl.SelectNodeByTag("Resources");
					}
					DictionaryControl.ExpandTreeNode(((TreeView)DictionaryControl.tvData).SelectedNode);
					DictionaryControl.SelectNodeByTag(stiResource);
					return;
				}
			}
			finally
			{
				((IDisposable)stiResourceEditForm)?.Dispose();
			}
			if (Designer.UndoRedo != null && Designer.UndoRedo.CanUndo)
			{
				Designer.UndoRedo.UndoStackPop();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemSaveResource_Click(object sender, EventArgs e)
	{
		TreeNode selectedNode = ((TreeView)DictionaryControl.tvData).SelectedNode;
		object obj = ((selectedNode != null) ? selectedNode.Tag : null);
		if (obj is StiResource stiResource)
		{
			StiResourceEditForm.SaveResource(stiResource.Type, stiResource.Content, stiResource.Name);
		}
	}

	private void ItemMakeThisRelationActive_Click(object sender, EventArgs e)
	{
		if (((TreeView)DictionaryControl.tvData).SelectedNode != null && ((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiDataRelation)
		{
			StiDataRelation relation = (StiDataRelation)((TreeView)DictionaryControl.tvData).SelectedNode.Tag;
			(from r in relation.ChildSource.ParentRelationList()
				where r != relation
				select r).ToList().ForEach(delegate(StiDataRelation r)
			{
				r.Active = false;
			});
			relation.Active = true;
			BuildDictionary();
			DictionaryControl.SelectNodeByTag(relation);
			StiCacheCleaner.Clean(Report);
		}
	}

	private void ButtonEdit_Click(object sender, EventArgs e)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Invalid comparison between Unknown and I4
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ad: Invalid comparison between Unknown and I4
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Invalid comparison between Unknown and I4
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_1364: Unknown result type (might be due to invalid IL or missing references)
		//IL_136a: Invalid comparison between Unknown and I4
		//IL_10ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f3: Invalid comparison between Unknown and I4
		//IL_0b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1a: Invalid comparison between Unknown and I4
		//IL_0e2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e31: Invalid comparison between Unknown and I4
		try
		{
			if (((TreeView)DictionaryControl.tvData).SelectedNode == null || !contextItemEdit.Visible)
			{
				return;
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiDatabase stiDatabase && InvokeConnectionEdit(stiDatabase))
			{
				foreach (TreeNode node in ((TreeView)DictionaryControl.tvData).Nodes)
				{
					TreeNode val = node;
					if (val.Tag == stiDatabase)
					{
						((TreeView)DictionaryControl.tvData).SelectedNode = val;
						break;
					}
				}
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiDataTransformation stiDataTransformation)
			{
				stiDataTransformation.Key = StiKeyHelper.GetOrGeneratedKey(stiDataTransformation.Key);
				InvokeDataTransformationEdit(stiDataTransformation);
				return;
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiDataSource)
			{
				Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "DataSourceEdit"));
				StiDataSource stiDataSource = ((TreeView)DictionaryControl.tvData).SelectedNode.Tag as StiDataSource;
				stiDataSource.Key = StiKeyHelper.GetOrGeneratedKey(stiDataSource.Key);
				StiDataAdapterService stiDataAdapterService = null;
				StiDataAdapterService stiDataAdapterService2 = null;
				foreach (StiDataAdapterService item in StiOptions.Services.DataAdapters.Where((StiDataAdapterService s) => s.ServiceEnabled))
				{
					if (item is StiBusinessObjectAdapterService)
					{
						stiDataAdapterService2 = item;
					}
					if (item.GetDataSourceType() == stiDataSource.GetType())
					{
						stiDataAdapterService = item;
						break;
					}
				}
				if (stiDataAdapterService == null && stiDataSource is StiBusinessObjectSource)
				{
					stiDataAdapterService = stiDataAdapterService2;
				}
				if (stiDataAdapterService != null)
				{
					string name = stiDataSource.Name;
					int count = Designer.Report.Dictionary.DataSources.Count;
					if (stiDataAdapterService.Edit(Designer.Report.Dictionary, stiDataSource))
					{
						if (stiDataSource.Name != name)
						{
							StiComponentsCollection components = Report.GetComponents();
							foreach (StiComponent item2 in components)
							{
								if (item2 is IStiDataSource stiDataSource2 && stiDataSource2.DataSourceName == name)
								{
									stiDataSource2.DataSourceName = stiDataSource.Name;
								}
							}
						}
						Designer.Report.Dictionary.Connect(loadData: false);
						BuildDictionary();
						DictionaryControl.SelectNodeByKey(stiDataSource.Key);
						StiCacheCleaner.Clean(Designer?.Report);
						return;
					}
					BuildDictionary();
					DictionaryControl.SelectNodeByKey(stiDataSource.Key);
				}
				else
				{
					StiLogService.Write(GetType(), "Data Adapter not found");
					MessageBox.Show("Data Adapter not found");
				}
				if (Designer.UndoRedo != null && Designer.UndoRedo.CanUndo)
				{
					Designer.UndoRedo.UndoStackPop();
				}
				return;
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiBusinessObject)
			{
				try
				{
					StiBusinessObject stiBusinessObject = ((TreeView)DictionaryControl.tvData).SelectedNode.Tag as StiBusinessObject;
					stiBusinessObject.Key = StiKeyHelper.GetOrGeneratedKey(stiBusinessObject.Key);
					StiBusinessObjectEditForm stiBusinessObjectEditForm = new StiBusinessObjectEditForm(stiBusinessObject, newForm: false);
					try
					{
						if (Report.Designer != null && Report.Designer.DesignerControl != null)
						{
							Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiBusinessObjectEditForm);
						}
						if ((int)((Form)stiBusinessObjectEditForm).ShowDialog() == 1)
						{
							BuildDictionary();
							DictionaryControl.SelectNodeByTag(stiBusinessObject);
						}
						else if (Designer.UndoRedo != null && Designer.UndoRedo.CanUndo)
						{
							Designer.UndoRedo.UndoStackPop();
						}
						return;
					}
					finally
					{
						((IDisposable)stiBusinessObjectEditForm)?.Dispose();
					}
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
					return;
				}
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiBusinessObjectCategory)
			{
				Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "CategoryEdit").Replace("&", string.Empty));
				StiCategoryEditForm stiCategoryEditForm = new StiCategoryEditForm(Designer, ((TreeView)DictionaryControl.tvData).SelectedNode.Text, isEditMode: false);
				try
				{
					Designer?.AddOwnedForm((Form)(object)stiCategoryEditForm);
					((Control)stiCategoryEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "CategoryEdit"));
					string text = ((TreeView)DictionaryControl.tvData).SelectedNode.Text;
					if ((int)((Form)stiCategoryEditForm).ShowDialog() == 1)
					{
						foreach (StiBusinessObject businessObject in Report.Dictionary.BusinessObjects)
						{
							if (businessObject.Category == text)
							{
								businessObject.Category = stiCategoryEditForm.NewCategoryName;
							}
						}
						((TreeView)DictionaryControl.tvData).SelectedNode.Text = stiCategoryEditForm.NewCategoryName;
						return;
					}
				}
				finally
				{
					((IDisposable)stiCategoryEditForm)?.Dispose();
				}
				if (Designer.UndoRedo != null && Designer.UndoRedo.CanUndo)
				{
					Designer.UndoRedo.UndoStackPop();
				}
				return;
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiDataRelation)
			{
				StiDataRelation relation = (StiDataRelation)((TreeView)DictionaryControl.tvData).SelectedNode.Tag;
				relation.Key = StiKeyHelper.GetOrGeneratedKey(relation.Key);
				bool active = relation.Active;
				StiDataRelationEditForm stiDataRelationEditForm = new StiDataRelationEditForm(isEditMode: true);
				try
				{
					Designer.AddOwnedForm((Form)(object)stiDataRelationEditForm);
					((Control)stiDataRelationEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "RelationEdit"));
					stiDataRelationEditForm.ReadRelation(relation);
					if ((int)((Form)stiDataRelationEditForm).ShowDialog() != 1)
					{
						return;
					}
					Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "RelationEdit"));
					if (stiDataRelationEditForm.CopyModeActivated)
					{
						relation = new StiDataRelation();
						DictionaryControl.Designer.Report.Dictionary.Relations.Add(relation);
					}
					if (relation.NameInSource != stiDataRelationEditForm.RelationNameInSource)
					{
						StiComponentsCollection components2 = Report.GetComponents();
						foreach (StiComponent item3 in components2)
						{
							if (item3 is IStiDataRelation stiDataRelation && stiDataRelation.DataRelationName == relation.NameInSource)
							{
								stiDataRelation.DataRelationName = stiDataRelationEditForm.RelationNameInSource;
							}
						}
					}
					stiDataRelationEditForm.WriteRelation(relation);
					if (stiDataRelationEditForm.CopyModeActivated)
					{
						relation.Active = false;
						string text2 = stiDataRelationEditForm.RelationName;
						if (DataSourceContainDataRelation(relation.ChildSource, text2))
						{
							int num = 1;
							while (true)
							{
								text2 = ((num == 1) ? (stiDataRelationEditForm.RelationName + StiLocalization.Get("Report", "CopyOf")) : string.Format("{0}{1}{2}", stiDataRelationEditForm.RelationName, StiLocalization.Get("Report", "CopyOf"), num));
								if (!DataSourceContainDataRelation(relation.ChildSource, text2))
								{
									break;
								}
								num++;
							}
						}
						if (relation.Name == relation.Alias)
						{
							relation.Alias = text2;
						}
						relation.Name = text2;
					}
					if (!active && relation.Active)
					{
						(from r in relation.ChildSource.ParentRelationList()
							where r != relation
							select r).ToList().ForEach(delegate(StiDataRelation r)
						{
							r.Active = false;
						});
					}
					BuildDictionary();
					DictionaryControl.SelectNodeByKey(relation.Key);
					StiCacheCleaner.Clean(Designer?.Report);
					return;
				}
				finally
				{
					((IDisposable)stiDataRelationEditForm)?.Dispose();
				}
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiCalcDataColumn)
			{
				StiCalcDataColumn stiCalcDataColumn = (StiCalcDataColumn)((TreeView)DictionaryControl.tvData).SelectedNode.Tag;
				stiCalcDataColumn.Key = StiKeyHelper.GetOrGeneratedKey(stiCalcDataColumn.Key);
				StiActions.Dictionary.InvokeCalcDataColumnEdit(this, stiCalcDataColumn, Designer);
				DictionaryControl.SelectNodeByKey(stiCalcDataColumn.Key);
				return;
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiDataParameter)
			{
				StiDataParameter stiDataParameter = (StiDataParameter)((TreeView)DictionaryControl.tvData).SelectedNode.Tag;
				stiDataParameter.Key = StiKeyHelper.GetOrGeneratedKey(stiDataParameter.Key);
				StiDataParameterEditForm stiDataParameterEditForm = new StiDataParameterEditForm(Designer.Report, isEditMode: true);
				try
				{
					if (Designer != null)
					{
						Designer.AddOwnedForm((Form)(object)stiDataParameterEditForm);
					}
					((Control)stiDataParameterEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "DataParameterEdit"));
					((Control)stiDataParameterEditForm.tbName).Text = stiDataParameter.Name;
					((Control)stiDataParameterEditForm.tbValue).Text = stiDataParameter.Value;
					Type parameterTypesEnum = ((StiSqlSource)stiDataParameter.DataSource).GetParameterTypesEnum();
					Array values = Enum.GetValues(parameterTypesEnum);
					object[] array = new object[values.Length];
					values.CopyTo(array, 0);
					((ComboBox)stiDataParameterEditForm.cbType).Items.AddRange(array);
					((ComboBox)stiDataParameterEditForm.cbType).Sorted = true;
					int num2 = 0;
					foreach (object item4 in ((ComboBox)stiDataParameterEditForm.cbType).Items)
					{
						if ((int)item4 == stiDataParameter.Type)
						{
							((ListControl)stiDataParameterEditForm.cbType).SelectedIndex = num2;
							break;
						}
						num2++;
					}
					if ((int)((Form)stiDataParameterEditForm).ShowDialog() != 1)
					{
						return;
					}
					Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "DataParameterEdit"));
					StiDataSource stiDataSource3 = null;
					if (stiDataParameterEditForm.CopyModeActivated)
					{
						stiDataSource3 = stiDataParameter.DataSource;
						stiDataParameter = new StiDataParameter();
						stiDataSource3.Parameters.Add(stiDataParameter);
					}
					stiDataParameter.Name = ((Control)stiDataParameterEditForm.tbName).Text;
					stiDataParameter.Value = ((Control)stiDataParameterEditForm.tbValue).Text;
					stiDataParameter.Type = (int)((ComboBox)stiDataParameterEditForm.cbType).SelectedItem;
					if (stiDataParameterEditForm.CopyModeActivated)
					{
						string name2 = ((Control)stiDataParameterEditForm.tbName).Text;
						if (stiDataSource3.Parameters.Contains(name2))
						{
							int num3 = 1;
							while (true)
							{
								name2 = ((num3 == 1) ? (((Control)stiDataParameterEditForm.tbName).Text + StiLocalization.Get("Report", "CopyOf")) : string.Format("{0}{1}{2}", ((Control)stiDataParameterEditForm.tbName).Text, StiLocalization.Get("Report", "CopyOf"), num3));
								if (!stiDataSource3.Parameters.Contains(name2))
								{
									break;
								}
								num3++;
							}
						}
						stiDataParameter.Name = name2;
					}
					BuildDictionary();
					DictionaryControl.SelectNodeByKey(stiDataParameter.Key);
					return;
				}
				finally
				{
					((IDisposable)stiDataParameterEditForm)?.Dispose();
				}
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiDataColumn)
			{
				StiDataColumn stiDataColumn = (StiDataColumn)((TreeView)DictionaryControl.tvData).SelectedNode.Tag;
				stiDataColumn.Key = StiKeyHelper.GetOrGeneratedKey(stiDataColumn.Key);
				StiDataColumnEditForm stiDataColumnEditForm = new StiDataColumnEditForm(isEditMode: true);
				try
				{
					Designer?.AddOwnedForm((Form)(object)stiDataColumnEditForm);
					((Control)stiDataColumnEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "ColumnEdit"));
					((Control)stiDataColumnEditForm.textBoxNameInSource).Text = stiDataColumn.NameInSource;
					((Control)stiDataColumnEditForm.tbName).Text = stiDataColumn.Name;
					((Control)stiDataColumnEditForm.tbAlias).Text = stiDataColumn.Alias;
					if (stiDataColumn is StiDataTransformationColumn)
					{
						stiDataColumnEditForm.Expression = ((StiDataTransformationColumn)stiDataColumn).Expression;
					}
					StiTypeWrapper[] typeWrappers = StiTypeWrapper.GetTypeWrappers();
					List<StiTypeWrapper> list = new List<StiTypeWrapper>(typeWrappers);
					int num4 = 0;
					int num5 = -1;
					StiTypeWrapper[] array2 = typeWrappers;
					foreach (StiTypeWrapper stiTypeWrapper in array2)
					{
						if ((stiDataColumn.Type != null && stiTypeWrapper.Type.ToString() == stiDataColumn.Type.ToString()) || (stiDataColumn.Type == null && stiTypeWrapper.Type == typeof(object)))
						{
							num5 = num4;
							break;
						}
						num4++;
					}
					if (num5 == -1)
					{
						list.Add(new StiTypeWrapper(stiDataColumn.Type));
						num5 = list.Count - 1;
					}
					ObjectCollection items = ((ComboBox)stiDataColumnEditForm.cbType).Items;
					object[] array3 = list.ToArray();
					items.AddRange(array3);
					((ListControl)stiDataColumnEditForm.cbType).SelectedIndex = num5;
					if ((int)((Form)stiDataColumnEditForm).ShowDialog() != 1)
					{
						return;
					}
					Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "ColumnEdit"));
					StiDataSource stiDataSource4 = null;
					if (stiDataColumnEditForm.CopyModeActivated)
					{
						stiDataSource4 = stiDataColumn.DataSource;
						stiDataColumn = ((stiDataColumn is StiDataTransformationColumn) ? new StiDataTransformationColumn() : new StiDataColumn());
						stiDataSource4.Columns.Add(stiDataColumn);
					}
					stiDataColumn.NameInSource = ((Control)stiDataColumnEditForm.textBoxNameInSource).Text;
					stiDataColumn.Name = ((Control)stiDataColumnEditForm.tbName).Text;
					stiDataColumn.Alias = ((Control)stiDataColumnEditForm.tbAlias).Text;
					stiDataColumn.Type = ((StiTypeWrapper)((ComboBox)stiDataColumnEditForm.cbType).SelectedItem).Type;
					if (stiDataColumn is StiDataTransformationColumn)
					{
						((StiDataTransformationColumn)stiDataColumn).Expression = stiDataColumnEditForm.Expression;
					}
					if (stiDataColumnEditForm.CopyModeActivated)
					{
						string text3 = ((Control)stiDataColumnEditForm.tbName).Text;
						if (stiDataSource4.Columns.Contains(text3))
						{
							int num6 = 1;
							while (true)
							{
								text3 = ((num6 == 1) ? (((Control)stiDataColumnEditForm.tbName).Text + StiLocalization.Get("Report", "CopyOf")) : string.Format("{0}{1}{2}", ((Control)stiDataColumnEditForm.tbName).Text, StiLocalization.Get("Report", "CopyOf"), num6));
								if (!stiDataSource4.Columns.Contains(text3))
								{
									break;
								}
								num6++;
							}
						}
						if (stiDataColumn.Name == stiDataColumn.Alias)
						{
							stiDataColumn.Alias = text3;
						}
						stiDataColumn.Name = text3;
					}
					BuildDictionary();
					DictionaryControl.SelectNodeByKey(stiDataColumn.Key);
					return;
				}
				finally
				{
					((IDisposable)stiDataColumnEditForm)?.Dispose();
				}
			}
			if (((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiVariable)
			{
				StiVariable stiVariable = (StiVariable)((TreeView)DictionaryControl.tvData).SelectedNode.Tag;
				stiVariable.Key = StiKeyHelper.GetOrGeneratedKey(stiVariable.Key);
				if (stiVariable.Name.Length > 0)
				{
					StiActions.Dictionary.InvokeVariableEdit(this, stiVariable, Designer);
					return;
				}
				Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "CategoryEdit").Replace("&", ""));
				StiCategoryEditForm stiCategoryEditForm2 = new StiCategoryEditForm(Designer, ((TreeView)DictionaryControl.tvData).SelectedNode.Text, isEditMode: true, isVariableMode: true);
				try
				{
					if (Designer != null)
					{
						Designer.AddOwnedForm((Form)(object)stiCategoryEditForm2);
					}
					((Control)stiCategoryEditForm2).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "CategoryEdit"));
					string text4 = ((TreeView)DictionaryControl.tvData).SelectedNode.Text;
					if ((int)((Form)stiCategoryEditForm2).ShowDialog() == 1)
					{
						if (!stiCategoryEditForm2.CopyModeActivated)
						{
							Designer.Report.Dictionary.Variables.RenameCategory(text4, stiCategoryEditForm2.NewCategoryName);
							((TreeView)DictionaryControl.tvData).SelectedNode.Text = stiCategoryEditForm2.NewCategoryName;
							BuildDictionary();
							DictionaryControl.SelectNodeByKey(stiVariable.Key);
							return;
						}
						string text5 = stiCategoryEditForm2.NewCategoryName;
						if (Designer.Report.Dictionary.Variables.ContainsCategory(text5))
						{
							int num7 = 1;
							while (true)
							{
								text5 = ((num7 == 1) ? (stiCategoryEditForm2.NewCategoryName + " " + StiLocalization.Get("Report", "CopyOf")) : string.Format("{0} {1} {2}", stiCategoryEditForm2.NewCategoryName, StiLocalization.Get("Report", "CopyOf"), num7));
								if (!Designer.Report.Dictionary.Variables.ContainsCategory(text5))
								{
									break;
								}
								num7++;
							}
						}
						StiVariable stiVariable2 = new StiVariable(text5);
						Designer.Report.Dictionary.Variables.Add(stiVariable2);
						BuildDictionary();
						DictionaryControl.SelectNodeByKey(stiVariable2.Key);
					}
				}
				finally
				{
					((IDisposable)stiCategoryEditForm2)?.Dispose();
				}
				if (Designer.UndoRedo != null && Designer.UndoRedo.CanUndo)
				{
					Designer.UndoRedo.UndoStackPop();
				}
			}
			else
			{
				if (!(((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiResource))
				{
					return;
				}
				StiResource stiResource = (StiResource)((TreeView)DictionaryControl.tvData).SelectedNode.Tag;
				stiResource.Key = StiKeyHelper.GetOrGeneratedKey(stiResource.Key);
				StiResourceEditForm stiResourceEditForm = new StiResourceEditForm(Designer.Report, isEditMode: true, stiResource.Type, stiResource.Content);
				try
				{
					if (Designer != null)
					{
						Designer.AddOwnedForm((Form)(object)stiResourceEditForm);
					}
					((Control)stiResourceEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "ResourceEdit"));
					((Control)stiResourceEditForm.textBoxName).Text = stiResource.Name;
					((Control)stiResourceEditForm.textBoxAlias).Text = stiResource.Alias;
					((CheckBox)stiResourceEditForm.checkBoxAvailableInTheViewer).Checked = stiResource.AvailableInTheViewer;
					if ((int)((Form)stiResourceEditForm).ShowDialog() != 1)
					{
						return;
					}
					Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "ResourceEdit"));
					if (stiResourceEditForm.CopyModeActivated)
					{
						stiResource = new StiResource();
						DictionaryControl.Designer.Report.Dictionary.Resources.Add(stiResource);
					}
					if (stiResource.Type == StiResourceType.Map)
					{
						StiMapLoader.DeleteAllCustomMaps();
						if (stiResource.Name != ((Control)stiResourceEditForm.textBoxName).Text)
						{
							StiComponentsCollection components3 = Designer.Report.GetComponents();
							foreach (object item5 in components3)
							{
								if (item5 is StiMap)
								{
									StiMap stiMap = (StiMap)item5;
									if (stiMap.MapIdent == stiResource.Name)
									{
										stiMap.MapIdent = ((Control)stiResourceEditForm.textBoxName).Text;
									}
								}
								if (item5 is IStiRegionMapElement)
								{
									IStiRegionMapElement stiRegionMapElement = (IStiRegionMapElement)item5;
									if (stiRegionMapElement.MapIdent == stiResource.Name)
									{
										stiRegionMapElement.MapIdent = ((Control)stiResourceEditForm.textBoxName).Text;
									}
								}
							}
						}
					}
					stiResource.Name = ((Control)stiResourceEditForm.textBoxName).Text;
					stiResource.Alias = ((Control)stiResourceEditForm.textBoxAlias).Text;
					stiResource.AvailableInTheViewer = ((CheckBox)stiResourceEditForm.checkBoxAvailableInTheViewer).Checked && ((Control)stiResourceEditForm.checkBoxAvailableInTheViewer).Enabled;
					stiResource.Type = stiResourceEditForm.ResourceType;
					stiResource.Content = stiResourceEditForm.Content;
					if (stiResourceEditForm.CopyModeActivated)
					{
						List<StiResource> source = Report.Dictionary.Resources.ToList();
						string resourceName = ((Control)stiResourceEditForm.textBoxName).Text;
						if (source.Any((StiResource f) => f.Name == resourceName))
						{
							int num8 = 1;
							while (true)
							{
								resourceName = ((Control)stiResourceEditForm.textBoxName).Text + StiLocalization.Get("Report", "CopyOf") + ((num8 == 1) ? string.Empty : num8.ToString());
								if (source.Count((StiResource f) => f.Name == resourceName) == 0)
								{
									break;
								}
								num8++;
							}
						}
						if (stiResource.Name == stiResource.Alias)
						{
							stiResource.Alias = resourceName;
						}
						stiResource.Name = resourceName;
					}
					BuildDictionary();
					DictionaryControl.SelectNodeByKey(stiResource.Key);
					return;
				}
				finally
				{
					((IDisposable)stiResourceEditForm)?.Dispose();
				}
			}
		}
		catch (Exception exception2)
		{
			StiExceptionProvider.Show(exception2);
		}
	}

	private void ItemProperties_Click(object sender, EventArgs e)
	{
		try
		{
			Designer.SelectedObject = ((TreeView)DictionaryControl.tvData).SelectedNode.Tag;
			StiDockingPanelSelector.PropertiesPanelSelect(Designer);
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemDuplicate_Click(object sender, EventArgs e)
	{
		StiActions.Dictionary.InvokeDuplicate(this);
	}

	private async void ButtonDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if ((int)MessageBox.Show(StiLocalization.Get("Questions", "qnRemove"), StiLocalization.Get("FormDictionaryDesigner", "title"), (MessageBoxButtons)4) == 6)
			{
				try
				{
					StiDictionaryPanelHelper.ShowProgress(Designer, Loc.GetCleaned("Cloud", "TextDeletingItems"));
					await Designer.UndoRedoSaveAsync(StiLocalization.Get("MainMenu", "menuEditDelete"));
					DeleteFromDictionary();
					return;
				}
				finally
				{
					StiDictionaryPanelHelper.HideProgress(Designer);
				}
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemExpandAll_Click(object sender, EventArgs e)
	{
		((TreeView)DictionaryControl.tvData).ExpandAll();
	}

	private void ItemCollapseAll_Click(object sender, EventArgs e)
	{
		((TreeView)DictionaryControl.tvData).CollapseAll();
	}

	private void ItemDictionaryNew_Click(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		try
		{
			if ((int)MessageBox.Show(StiLocalization.Get("Questions", "qnDictionaryNew"), StiLocalization.Get("FormDictionaryDesigner", "title"), (MessageBoxButtons)4) == 6)
			{
				Designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuFileNew"));
				Designer.Report.Dictionary.Clear();
				BuildDictionary();
				DictionaryCheck();
				StiCacheCleaner.Clean(Designer?.Report);
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemDictionaryOpen_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			try
			{
				((FileDialog)val).Filter = StiSLService.GetDictionaryFilters(StiSLActions.Load);
				((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileOpen"));
				((FileDialog)val).RestoreDirectory = true;
				if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName != "")
				{
					Designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuFileOpen"));
					Designer.Report.Dictionary.Load(StiSLService.GetDictionarySLServices(StiSLActions.Load)[((FileDialog)val).FilterIndex - 1], ((FileDialog)val).FileName);
					BuildDictionary();
					StiCacheCleaner.Clean(Designer?.Report);
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

	private void ItemDictionarySave_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Invalid comparison between Unknown and I4
		try
		{
			SaveFileDialog val = new SaveFileDialog();
			try
			{
				((FileDialog)val).Filter = StiSLService.GetDictionaryFilters(StiSLActions.Save);
				((FileDialog)val).RestoreDirectory = true;
				((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileSaveAs"));
				((FileDialog)val).FileName = StiLocalization.Get("FormDictionaryDesigner", "title");
				((FileDialog)val).DefaultExt = "dct";
				if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName != "")
				{
					Designer.Report.Dictionary.Save(StiSLService.GetDictionarySLServices(StiSLActions.Save)[((FileDialog)val).FilterIndex - 1], ((FileDialog)val).FileName);
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

	private void ItemDictionaryMerge_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			try
			{
				((FileDialog)val).Filter = StiSLService.GetDictionaryFilters(StiSLActions.Merge);
				((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileMerge"));
				((FileDialog)val).RestoreDirectory = true;
				if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName != "")
				{
					Designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuFileMerge"));
					Designer.Report.Dictionary.Merge(StiSLService.GetDictionarySLServices(StiSLActions.Merge)[((FileDialog)val).FilterIndex - 1], ((FileDialog)val).FileName);
					BuildDictionary();
					StiCacheCleaner.Clean(Designer?.Report);
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

	private void ItemDictionaryXmlMerge_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			try
			{
				((FileDialog)val).Filter = "DataSet XML schema (*.xsd)|*.xsd";
				((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileMergeXMLSchema"));
				((FileDialog)val).RestoreDirectory = true;
				if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName != "")
				{
					Designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuFileMergeXMLSchema"));
					DataSet dataSet = new DataSet("Test")
					{
						EnforceConstraints = false
					};
					dataSet.ReadXmlSchema(((FileDialog)val).FileName);
					StiOptions.Engine.GlobalEvents.InvokeImportXmlSchema(Designer, new StiOptions.Engine.GlobalEvents.StiImportXmlSchemaEventArg(dataSet));
					Designer.Report.Dictionary.MergeXMLSchema(dataSet);
					BuildDictionary();
					StiCacheCleaner.Clean(Designer?.Report);
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

	private void ItemDictionaryXmlImport_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			try
			{
				((FileDialog)val).Filter = "DataSet XML schema (*.xsd)|*.xsd";
				((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileImportXMLSchema"));
				((FileDialog)val).RestoreDirectory = true;
				if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName != "")
				{
					Designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuFileImportXMLSchema"));
					DataSet dataSet = new DataSet("Test")
					{
						EnforceConstraints = false
					};
					dataSet.ReadXmlSchema(((FileDialog)val).FileName);
					StiOptions.Engine.GlobalEvents.InvokeImportXmlSchema(Designer, new StiOptions.Engine.GlobalEvents.StiImportXmlSchemaEventArg(dataSet));
					Designer.Report.Dictionary.ImportXMLSchema(dataSet);
					BuildDictionary();
					StiCacheCleaner.Clean(Designer?.Report);
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

	private void ItemDictionaryXmlExport_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Invalid comparison between Unknown and I4
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Invalid comparison between Unknown and I4
		try
		{
			SaveFileDialog val = new SaveFileDialog();
			try
			{
				((FileDialog)val).Filter = StiLocalization.Get("FileFilters", "DataSetXmlSchema");
				((FileDialog)val).RestoreDirectory = true;
				((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileExportXMLSchema"));
				((FileDialog)val).FileName = "schema";
				((FileDialog)val).DefaultExt = "xml";
				if ((int)((CommonDialog)val).ShowDialog() != 1 || !(((FileDialog)val).FileName != ""))
				{
					return;
				}
				StiDataSetNameForm stiDataSetNameForm = new StiDataSetNameForm();
				try
				{
					Designer.AddOwnedForm((Form)(object)stiDataSetNameForm);
					if ((int)((Form)stiDataSetNameForm).ShowDialog() == 1)
					{
						Designer.Report.Dictionary.ExportXMLSchema(((Control)stiDataSetNameForm.tbDataSetName).Text).WriteXmlSchema(((FileDialog)val).FileName);
					}
				}
				finally
				{
					((IDisposable)stiDataSetNameForm)?.Dispose();
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

	private void ItemMarkUsed_Click(object sender, EventArgs e)
	{
		try
		{
			DictionaryControl.Build(Report, showUsedObjects: true);
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemRemoveUnused_Click(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		try
		{
			if ((int)MessageBox.Show(StiLocalization.Get("Questions", "qnRemoveUnused"), StiLocalization.Get("FormDictionaryDesigner", "title"), (MessageBoxButtons)4) == 6)
			{
				Designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuEditRemoveUnused"));
				Designer.Report.Dictionary.RemoveUnusedData();
				BuildDictionary();
				StiCacheCleaner.Clean(Designer?.Report);
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonActionsMenu_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		StiOptions.Engine.GlobalEvents.InvokeDictionaryMenuActionsOpenInDesigner(sender);
		BusinessObjectsCheck();
		RelationCheck();
	}

	private void ButtonNewMenu_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		StiOptions.Engine.GlobalEvents.InvokeDictionaryMenuNewOpenInDesigner(sender);
		BusinessObjectsCheck();
		RelationCheck();
	}

	private void ButtonEditMenu_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		foreach (object subItem in contextItemEditMenu.SubItems)
		{
			if (subItem is BaseItem baseItem)
			{
				baseItem.Enabled = Report != null;
			}
		}
		StiOptions.Engine.GlobalEvents.InvokeDictionaryMenuEditOpenInDesigner(sender);
		BusinessObjectsCheck();
		RelationCheck();
	}

	private void ItemOption_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (Designer == null || Designer.DesignerConfig == null)
			{
				return;
			}
			if (sender == itemCreateField)
			{
				Designer.CreateFieldOnDoubleClick = itemCreateField.Checked;
			}
			else if (sender == itemUseAliases)
			{
				Designer.UseAliases = itemUseAliases.Checked;
				if (!Designer.UseAliases)
				{
					Designer.TextAliasesHash.Clear();
				}
				BuildDictionary();
			}
			else
			{
				Designer.CreateLabel = itemCreateLabel.Checked;
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void Designer_RefreshDictionary(object sender, EventArgs e)
	{
		if (Report != null)
		{
			DictionaryControl.CanBuildFormats = false;
			UpdateDictionaryEmptyState();
			DictionaryControl.Build(Report);
			if (itemCreateField != null)
			{
				itemCreateField.Enabled = true;
			}
			((Control)DictionaryControl).Enabled = true;
			((Control)toolBarMain).Enabled = true;
		}
		else
		{
			if (itemCreateField != null)
			{
				itemCreateField.Enabled = false;
			}
			((Control)DictionaryControl).Enabled = false;
			((Control)toolBarMain).Enabled = false;
		}
		UpdateDictionaryEmptyState();
	}

	private void TreeViewData_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (((TreeView)DictionaryControl.tvData).SelectedNode == null)
			{
				return;
			}
			if (Designer.CreateFieldOnDoubleClick)
			{
				if (!(((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiDataColumn) && !(((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiDataParameter) && !(((TreeView)DictionaryControl.tvData).SelectedNode.Tag is StiResource))
				{
					return;
				}
				StiDataColumn stiDataColumn = ((TreeView)DictionaryControl.tvData).SelectedNode.Tag as StiDataColumn;
				StiDataParameter stiDataParameter = ((TreeView)DictionaryControl.tvData).SelectedNode.Tag as StiDataParameter;
				StiResource stiResource = ((TreeView)DictionaryControl.tvData).SelectedNode.Tag as StiResource;
				StiDataSource stiDataSource = null;
				if (stiDataColumn != null)
				{
					stiDataSource = stiDataColumn.DataSource;
				}
				if (stiDataParameter != null)
				{
					stiDataSource = stiDataParameter.DataSource;
				}
				StiDataBand stiDataBand = null;
				StiDataBand stiDataBand2 = null;
				StiDataBand stiDataBand3 = null;
				List<StiDataBand> list = new List<StiDataBand>();
				StiComponentsCollection components = Designer.Report.GetCurrentPage().GetComponents();
				foreach (StiComponent item in components)
				{
					if (item is StiDataBand stiDataBand4)
					{
						if (stiDataBand2 == null)
						{
							stiDataBand2 = stiDataBand4;
						}
						if (stiDataBand3 == null && stiDataBand4.IsSelected)
						{
							stiDataBand3 = stiDataBand4;
						}
						if (stiDataBand4.DataSource == stiDataSource)
						{
							list.Add(stiDataBand4);
						}
					}
				}
				if (list.Count > 0)
				{
					foreach (StiDataBand item2 in list)
					{
						if (stiDataBand == null)
						{
							stiDataBand = item2;
						}
						else if (!stiDataBand.IsSelected && item2.IsSelected)
						{
							stiDataBand = item2;
							break;
						}
					}
				}
				if (stiDataBand == null)
				{
					stiDataBand = stiDataBand3 ?? stiDataBand2;
				}
				if (stiDataBand != null)
				{
					StiBaseStyle stiBaseStyle = new StiStyle();
					double num = 0.0;
					foreach (StiComponent component in stiDataBand.Components)
					{
						if (num < component.Right)
						{
							num = component.Right;
							if (!(component is IStiIgnoryStyle))
							{
								stiBaseStyle = StiBaseStyle.GetStyle(component);
							}
						}
					}
					if (stiDataBand.Components.Count == 0 && Designer.UseLastFormat)
					{
						stiBaseStyle = Designer.CurrentStyle;
					}
					if (stiResource != null)
					{
						Designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Create") + " " + StiLocalization.Get("PropertyMain", "Resource"), onlyCurrentPage: true);
					}
					else
					{
						Designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Create") + " " + StiLocalization.Get("Components", "StiText"), onlyCurrentPage: true);
					}
					double width = StiAlignValue.AlignToMaxGrid(stiDataBand.Width / 8.0, Designer.Report.GetCurrentPage().GridSize, aligningToGrid: true);
					RectangleD rectangleD = new RectangleD(num, 0.0, width, stiDataBand.Height);
					StiText stiText = ((stiResource == null) ? (StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text, new object[1] { rectangleD }) as StiText) : null);
					StiImage stiImage = ((stiResource != null) ? (StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Image, new object[1] { rectangleD }) as StiImage) : null);
					if (stiText != null)
					{
						stiBaseStyle.SetStyleToComponent(stiText);
					}
					if (stiImage != null)
					{
						stiBaseStyle.SetStyleToComponent(stiImage);
					}
					if (stiResource != null)
					{
						stiImage.ImageURL.Value = StiHyperlinkProcessor.CreateResourceName(stiResource.Name);
						stiDataBand.Components.Add(stiImage);
					}
					else if (stiDataColumn != null)
					{
						stiText.Text = $"{{{StiDataBuilder.GetColumnPathFromNode(((TreeView)DictionaryControl.tvData).SelectedNode)}}}";
						stiText.Type = StiSystemTextType.DataColumn;
						stiDataBand.Components.Add(stiText);
					}
					else
					{
						stiText.Text = $"{{{StiDataBuilder.GetParameterPathFromNode(((TreeView)DictionaryControl.tvData).SelectedNode)}}}";
						stiText.Type = StiSystemTextType.DataColumn;
						stiDataBand.Components.Add(stiText);
					}
					StiDataBandV1Builder stiDataBandV1Builder = StiV1Builder.GetBuilder(typeof(StiDataBand)) as StiDataBandV1Builder;
					StiComponentsCollection headers = stiDataBandV1Builder.GetHeaders(stiDataBand);
					foreach (StiHeaderBand item3 in headers)
					{
						num = 0.0;
						foreach (StiComponent component2 in item3.Components)
						{
							if (num < component2.Right)
							{
								num = component2.Right;
								if (!(component2 is IStiIgnoryStyle))
								{
									stiBaseStyle = StiBaseStyle.GetStyle(component2);
								}
							}
						}
						stiText = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text, new object[1] { rectangleD }) as StiText;
						stiText.Height = item3.Height;
						stiText.HorAlignment = StiTextHorAlignment.Center;
						stiBaseStyle.SetStyleToComponent(stiText);
						if (stiResource != null)
						{
							stiText.Text = ((StiResource)((TreeView)DictionaryControl.tvData).SelectedNode.Tag).Alias;
						}
						else if (stiDataColumn != null)
						{
							stiText.Text = ((StiDataColumn)((TreeView)DictionaryControl.tvData).SelectedNode.Tag).Alias;
						}
						else
						{
							string text = ((StiDataParameter)((TreeView)DictionaryControl.tvData).SelectedNode.Tag).Name;
							if (text.StartsWith("@", StringComparison.InvariantCulture))
							{
								text = text.Substring(1);
							}
							stiText.Text = text;
						}
						item3.Components.Add(stiText);
					}
				}
				((Control)Designer).Refresh();
			}
			else
			{
				((TreeView)DictionaryControl.tvData).SelectedNode = ((TreeView)DictionaryControl.tvData).GetNodeAt(((Control)DictionaryControl.tvData).PointToClient(Cursor.Position));
				if (buttonEdit.Enabled)
				{
					ButtonEdit_Click(sender, EventArgs.Empty);
				}
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void TreeViewData_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 46 && buttonDelete.Enabled)
		{
			e.Handled = true;
			ButtonDelete_Click(sender, (EventArgs)(object)e);
		}
	}

	private void TreeViewData_AfterSelect(object sender, TreeViewEventArgs e)
	{
		DictionaryCheck();
		if (e.Node != null && e.Node.Tag != null && buttonEdit.Enabled && !StiInplaceEditor.IsEditorActivated)
		{
			Designer.SelectedObject = e.Node.Tag;
			Designer.InvokeSelectedObjectsChanged();
		}
	}

	private void ButtonUp_Click(object sender, EventArgs e)
	{
		try
		{
			TreeNode selectedNode = ((TreeView)DictionaryControl.tvData).SelectedNode;
			if (selectedNode != null && selectedNode.Tag != null)
			{
				int num = selectedNode.Parent.Nodes.IndexOf(selectedNode) - 1;
				MoveNode(selectedNode, selectedNode.Parent.Nodes[num]);
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonDown_Click(object sender, EventArgs e)
	{
		try
		{
			TreeNode selectedNode = ((TreeView)DictionaryControl.tvData).SelectedNode;
			if (selectedNode != null && selectedNode.Tag != null)
			{
				int num = selectedNode.Parent.Nodes.IndexOf(selectedNode) + 1;
				MoveNode(selectedNode, selectedNode.Parent.Nodes[num]);
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemSortNone_Click(object sender, EventArgs e)
	{
		try
		{
			itemSortAsc.Checked = false;
			itemSortDesc.Checked = false;
			itemSortNone.Checked = true;
			if (CurrentSort != StiDictionarySortOrder.None)
			{
				StiSettings.Load();
				StiSettings.Set("Designer", "DictSortType", 2);
				StiSettings.Save();
				CurrentSort = StiDictionarySortOrder.None;
				Report.Dictionary.SortItems();
				BuildDictionary();
				StiSettings.Load();
				StiSettings.Save();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemSortAsc_Click(object sender, EventArgs e)
	{
		try
		{
			StiSettings.Load();
			StiSettings.Set("Designer", "DictSortType", 0);
			StiSettings.Save();
			Designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Sort"));
			itemSortAsc.Checked = true;
			itemSortDesc.Checked = false;
			itemSortNone.Checked = false;
			CurrentSort = StiDictionarySortOrder.Asc;
			Report.Dictionary.SortItems(StiSortOrder.Asc);
			BuildDictionary();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemSortDesc_Click(object sender, EventArgs e)
	{
		try
		{
			StiSettings.Load();
			StiSettings.Set("Designer", "DictSortType", 1);
			StiSettings.Save();
			Designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Sort"));
			itemSortAsc.Checked = false;
			itemSortDesc.Checked = true;
			itemSortNone.Checked = false;
			CurrentSort = StiDictionarySortOrder.Desc;
			Report.Dictionary.SortItems(StiSortOrder.Desc);
			BuildDictionary();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonShowMore_Click(object sender, EventArgs e)
	{
		isShowMore = false;
		UpdateDictionaryEmptyState();
	}

	public void ShowProgress(string message = "", bool useTimer = true, string processGuid = null)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		isProgressActive = true;
		if (!string.IsNullOrEmpty(processGuid) && !processGuidList.Contains(processGuid))
		{
			processGuidList.Add(processGuid);
		}
		if (useTimer)
		{
			if (showProgressTimer == null)
			{
				showProgressTimer = new Timer();
			}
			showProgressTimer.Stop();
			showProgressTimer.Interval = 250;
			showProgressTimer.Start();
			showProgressTimer.Tick += delegate
			{
				if (isProgressActive)
				{
					Timer obj = showProgressTimer;
					if (obj != null)
					{
						obj.Stop();
					}
					ShowProgressImmediately(message);
				}
			};
		}
		else
		{
			ShowProgressImmediately(message);
		}
	}

	private void ShowProgressImmediately(string message)
	{
		try
		{
			isProgressActive = true;
			CreateProgress();
			((Control)labelOperation).Invoke((Delegate)(Action)delegate
			{
				((Control)labelOperation).Text = message;
			});
			CalculateProgressLocation();
			progressOperation.Value = 0;
			progressOperation.IsRunning = true;
			toolBarMain.Visible = false;
			((Control)controlNoData).Visible = false;
			((Control)DictionaryControl).Visible = false;
			((Control)panelOperation).Visible = true;
			((Control)panelContent).AllowDrop = false;
			((Control)Designer).AllowDrop = false;
			StiPageViewControl currentPageView = Designer.PageView.GetCurrentPageView();
			if (currentPageView != null)
			{
				((Control)currentPageView.View).AllowDrop = false;
			}
			((Control)labelOperation).Visible = !string.IsNullOrWhiteSpace(((Control)labelOperation).Text);
			if (controlDropFiles != null)
			{
				((Control)controlDropFiles).Visible = false;
			}
		}
		catch
		{
		}
	}

	public void UpdateMessage(string message)
	{
		if (isProgressActive)
		{
			((Control)labelOperation).Invoke((Delegate)(Action)delegate
			{
				((Control)labelOperation).Text = message;
			});
		}
	}

	public void HideProgress(string processGuid = null)
	{
		try
		{
			if (!string.IsNullOrEmpty(processGuid) && processGuidList.Contains(processGuid))
			{
				processGuidList.Remove(processGuid);
			}
			if (processGuidList.Count > 0)
			{
				return;
			}
			isProgressActive = false;
			if (progressOperation != null)
			{
				((Control)panelOperation).Visible = false;
				progressOperation.IsRunning = false;
				if (toolBarMain != null)
				{
					toolBarMain.Visible = true;
				}
				if (DictionaryControl != null)
				{
					((Control)DictionaryControl).Visible = true;
				}
				if (panelContent != null)
				{
					((Control)panelContent).AllowDrop = StiOptions.Designer.AllowUseDragDrop;
				}
				if (Designer != null)
				{
					((Control)Designer).AllowDrop = true;
				}
				StiViewControl stiViewControl = Designer?.PageView?.GetCurrentPageView()?.View;
				if (stiViewControl != null)
				{
					((Control)stiViewControl).AllowDrop = StiOptions.Designer.AllowUseDragDrop;
				}
			}
		}
		catch
		{
		}
	}

	private void CreateProgress()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		try
		{
			if (progressOperation == null)
			{
				panelOperation = new Panel();
				((Control)panelOperation).BackColor = Color.White;
				((Control)panelOperation).Dock = (DockStyle)5;
				((Control)panelOperation).Visible = false;
				((Control)base.DockingContainer).Controls.Add((Control)(object)panelOperation);
				((Control)panelOperation).BringToFront();
				progressOperation = new CircularProgress();
				((Control)progressOperation).BackColor = Color.White;
				((Control)progressOperation).Margin = new Padding(6);
				progressOperation.ProgressBarType = eCircularProgressType.Donut;
				progressOperation.ProgressColor = Color.SteelBlue;
				((Control)progressOperation).Size = new Size(StiScale.XXI(52.0), StiScale.YYI(52.0));
				progressOperation.Style = eDotNetBarStyle.OfficeXP;
				progressOperation.BackgroundStyle.CornerType = eCornerType.Square;
				((Control)panelOperation).Controls.Add((Control)(object)progressOperation);
				labelOperation = new Label();
				((Control)labelOperation).ForeColor = Color.DimGray;
				((Control)labelOperation).Height = StiScale.YYI(24.0);
				((Control)labelOperation).BackColor = Color.White;
				((Control)labelOperation).AutoSize = false;
				labelOperation.TextAlign = (ContentAlignment)32;
				((Control)panelOperation).Controls.Add((Control)(object)labelOperation);
				((Control)base.DockingContainer).SizeChanged += DockingContainer_SizeChanged;
			}
		}
		catch
		{
		}
	}

	private void CalculateProgressLocation()
	{
		try
		{
			if (base.DockingContainer != null && progressOperation != null && labelOperation != null)
			{
				int num = ((!string.IsNullOrWhiteSpace(((Control)labelOperation).Text)) ? ((Control)labelOperation).Height : 0);
				((Control)progressOperation).Left = (((Control)base.DockingContainer).ClientSize.Width - ((Control)progressOperation).Width) / 2;
				((Control)progressOperation).Top = (((Control)base.DockingContainer).ClientSize.Height - (((Control)progressOperation).Height + num)) / 2;
				((Control)labelOperation).Left = StiScale.I10;
				((Control)labelOperation).Top = ((Control)progressOperation).Bottom + StiScale.I4;
				((Control)labelOperation).Width = ((Control)base.DockingContainer).ClientSize.Width - StiScale.I10 * 2;
			}
		}
		catch
		{
		}
	}

	private void DisposeProgress()
	{
		if (progressOperation != null)
		{
			HideProgress();
			((Component)(object)panelOperation)?.Dispose();
			panelOperation = null;
			((Component)(object)progressOperation)?.Dispose();
			progressOperation = null;
			if (base.DockingContainer != null)
			{
				((Control)base.DockingContainer).SizeChanged -= DockingContainer_SizeChanged;
			}
			Timer obj = showProgressTimer;
			if (obj != null)
			{
				obj.Stop();
			}
			((Component)(object)showProgressTimer)?.Dispose();
			showProgressTimer = null;
		}
	}

	private void DockingContainer_SizeChanged(object sender, EventArgs e)
	{
		CalculateProgressLocation();
	}
}
