using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace Stimulsoft.Editor;

public class StringConsts
{
	public static string EmptyColor = "None";

	public static string ErrorCaption = "Error";

	public static string DefaultHyperTextHint = "{0}<BR>Ctrl + Click to follow link";

	public static string DefaultDragMarginHint = "Ctrl + Click to drag the margin";

	public static string NofMstr = "{0} of {1}";

	public static string LineNofMstr = "Line <b>{0}</b> of {1}";

	public static string LineStr = "Line";

	public static string PageNofMstr = "Page <b>{0}</b> of {1}";

	public static string PageStr = "Page";

	public static string InvalidTypeName = "Invalid Type Name: {0}";

	public static string InvalidPropName = "Invalid Property Name: {0}";

	public static string InvalidTabStop = "Invalid Tab Stops: {0}";

	public static string InvalidMarginColumn = "Invalid Column margins: {0}";

	public static string InvalidReference = "Invalid Object Reference: {0}";

	public static string InvalidRange = "Can't create intersecting outline section. Range :{0}, {1}";

	public static string StringNotFound = "The specified text was not found. Text : {0}";

	public static string SearchReachedStartPos = "Find reached the starting point of the search. Text : {0}";

	public static string ErrNamespaceDeclaration = "A namespace can only contain types and namespace declarations";

	public static string ErrNamespaceModifiers = "Namespace elements cannot be explicitly declared as private, protected or protected internal";

	public static string ErrNamespaceModifier = "A namespace declaration cannot have modifiers or attributes";

	public static string ErrArglistParameter = "An arglist parameter must be the last parameter in a formal parameter list";

	public static string ErrArrayParamSpecifier = "Array type specifier, [], must appear before parameter name";

	public static string ErrDefaultParamNotPermitted = "Default parameter specifiers are not permitted";

	public static string ErrStackallocExpression = "A stackalloc expression requires [] after type";

	public static string ErrInterfaceDeclaration = "interfaces can contain properties, methods and events only";

	public static string ErrInterfaceMemberDeclaration = "interface members cannot have a definition";

	public static string ErrSyntax = "Syntax error";

	public static string ErrExpected = "expected";

	public static string ErrIdentifierExpected = "Identifier expected";

	public static string ErrInExpected = "in expected";

	public static string ErrCatchOrFinallyExpected = "catch or finally expected";

	public static string ErrFixedPointerDeclaration = "You must provide an initializer in a fixed or using statement declaration";

	public static string ErrNewExpression = "new expression requires () or [] after type";

	public static string ErrWhileExpected = "while expected";

	public static string ErrTypeExpected = "Type expected";

	public static string ErrEndOfFileFound = "End of file found";

	public static string MethodOverloads = "(+ {0} overloads)";

	public static string CodeSnippetsEditField = "Insert Snippet: ";

	public static string CodeSnippetTooltipHeader = "Shorcut: {0}";

	public static string InsertDelegateHint = "new {0}({0});";

	public static string InsertDelegateExtendedHint = "<b>new {0}({0});</b> (Press Tab to insert)";

	public static string LineNumberCaption = "Line number ({0} - {1})";

	public static string DlgGotoCaption = "Goto Line";

	public static string EnterNewLineCaption = "Line number:";

	public static string OKCaption_GotoDlg = "OK";

	public static string CancelCaption_GotoDlg = "Cancel";

	public static string PrintOptionsButtonText = "&Options";

	public static string DlgPrintOptionsCaption = "Print Settings";

	public static string FileToPrintCaption = "File to Print:";

	public static string OptionsCaption = "Options:";

	public static string FileNameCaption = "File Name:";

	public static string PrintSelectionCaption = "Print selection";

	public static string UseHeaderCaption = "Header";

	public static string UseFooterCaption = "Footer";

	public static string LineNumbersCaption_PrintOptionsDlg = "Line Numbers";

	public static string PageNumbersCaption = "Page Numbers";

	public static string WordWrapCaption_PrintOptionsDlg = "Word Wrap";

	public static string UseSyntaxCaption = "Syntax print";

	public static string UseColorsCaption = "Color print";

	public static string DisplayProgressCaption = "Display progress";

	public static string OkCaption_PrintOptionsDlg = "OK";

	public static string CancelCaption_PrintOptionsDlg = "Cancel";

	public static string DlgSearchCaption = "Find";

	public static string DlgReplaceCaption = "Replace";

	public static string FindWhatCaption = "Fi&nd what:";

	public static string ReplaceWithCaption = "Re&place with:";

	public static string MatchCaseCaption = "Match &case";

	public static string MatchWholeWordCaption = "Match &whole word";

	public static string SearchHiddenTextCaption = "Search &hidden text";

	public static string SearchUpCaption = "Search &up";

	public static string UseRegularExpressionsCaption = "Use &regular expressions";

	public static string PromptOnReplaceCaption = "Prompt on replace";

	public static string SearchCaption = "Search";

	public static string FromCursorCaption = "From cursor";

	public static string EntireScopeCaption = "&Entire scope";

	public static string SelectionOnlyCaption = "Selection &Only";

	public static string FindNextCaption = "&Find Next";

	public static string ReplaceCaption = "&Replace";

	public static string ReplaceAllCaption = "Replace &All";

	public static string MarkAllCaption = "&Mark All";

	public static string CloseCaption = "Close";

	public static string HelpCaption = "Help";

	public static string SingleCharCaption = ". Any single character";

	public static string ZeroOrMoreCaption = "* Zero or more";

	public static string OneOrMoreCaption = "+ One or more";

	public static string BeginLineCaption = "^ Beginning of line";

	public static string EndLineCaption = "$ End of line";

	public static string OneCharInSetCaption = "[] Any one character in the set";

	public static string OneCharNotInSetCaption = "[^] Any one character not in the set";

	public static string OrCaption = "| Or";

	public static string EscapeCaption = "\\ Escape Special Character";

	public static string TagCaption = "{} Tag expression";

	public static string FindWhatTextCaption = "$0 Find What Text";

	public static string TagExpressionCaption = "Tagged Expression";

	public static string ConfirmReplaceText = "Replace this occurrence of {0}";

	public static string OccurencesReplaced = "{0} occurence(s) replaced.";

	public static string ConfirmReplace = "Confirm";

	public static string IdentStyle = "idents";

	public static string NumberStyle = "numbers";

	public static string ReswordStyle = "reswords";

	public static string CommentStyle = "comments";

	public static string XmlCommentStyle = "xmlcomments";

	public static string SymbolStyle = "symbols";

	public static string WhitespaceStyle = "whitespace";

	public static string StringStyle = "strings";

	public static string DirectiveStyle = "directives";

	public static string SyntaxErrorStyle = "syntaxerrors";

	public static string IdentsName = "Identifiers";

	public static string NumbersName = "Numbers";

	public static string ReswordsName = "ResWords";

	public static string CommentsName = "Comments";

	public static string StringsName = "Strings";

	public static string WhiteSpaceName = "WhiteSpace";

	public static string DirectivesName = "Directives";

	public static string XMLCommentsName = "XML Comments";

	public static string HTMLTagsName = "HTML tags";

	public static string LineNumbersName = "Line Numbers";

	public static string HyperTextName = "HyperText";

	public static string MisspelledWordsName = "Misspelled Words";

	public static string SymbolsName = "Symbols";

	public static string BracesName = "Braces";

	public static string OperatorsName = "Operators";

	public static string FunctionsName = "Functions";

	public static string ObjectsName = "Objects";

	public static string NewObjectsName = "New Objects";

	public static string SyntaxErrorsName = "Syntax Errors";

	public static string CodeSnippetsName = "Code Snippets";

	public static string LineModificatorChangedName = "Line Modificator Changed";

	public static string LineModificatorSavedName = "Line Modificator Saved";

	public static string LineSeparatorName = "Line Separator";

	public static string WindowColorName = "Window Color";

	public static string BraceMatchingName = "Brace Matching";

	public static string GutterPenColorName = "Gutter Pen Color";

	public static string CodeOutlinePenColorName = "Code Outline Pen Color";

	public static string ActiveSelectionName = "Active Selection Color";

	public static string InactiveSelectionName = "Inactive Selection Color";

	public static string IdentsInternalName = "idents";

	public static string NumbersInternalName = "numbers";

	public static string ReswordsInternalName = "reswords";

	public static string CommentsInternalName = "comments";

	public static string StringsInternalName = "strings";

	public static string WhiteSpaceInternalName = "whitespace";

	public static string DirectivesInternalName = "directives";

	public static string XMLCommentsInternalName = "xmlcomments";

	public static string HTMLTagsInternalName = "htmltags";

	public static string LineNumbersInternalName = "linenumbers";

	public static string HyperTextInternalName = "hypertext";

	public static string MisspelledWordsInternalName = "spelling";

	public static string SyntaxErrorsInternalName = "syntax errors";

	public static string CodeSnippetsInternalName = "codes nippets";

	public static string SymbolsInternalName = "symbols";

	public static string BracesInternalName = "braces";

	public static string OperatorsInternalName = "operators";

	public static string FunctionsInternalName = "functions";

	public static string ObjectsInternalName = "objects";

	public static string NewObjectsInternalName = "newobjects";

	public static string LineModificatorChangedInternalName = "linemodificatorchanged";

	public static string LineModificatorSavedInternalName = "linemodificatorsaved";

	public static string LineSeparatorInternalName = "lineseparator";

	public static string WindowColorInternalName = "windowcolor";

	public static string BraceMatchingInternalName = "bracematching";

	public static string GutterPenColorInternalName = "gutterpencolor";

	public static string CodeOutlinePenColorInternalName = "codeoutlinepencolor";

	public static string ActiveSelectionInternalName = "activeselectioncolor";

	public static string InactiveSelectionInternalName = "inactiveselectioncolor";

	public static string EmptyOption = "None";

	public static string DlgSyntaxSettingsCaption = "Syntax Settings";

	public static string PropertiesOptionsCaption = "Options";

	public static string PropertiesGeneralCaption = "General";

	public static string PropertiesFontsColorsCaption = "Fonts and Colors";

	public static string PropertiesAdditionalCaption = "Additional";

	public static string PropertiesKeyboradCaption = "Keyboard";

	public static string GeneralCaption = "General";

	public static string FontsAndColorsCaption = "Fonts&&Colors";

	public static string AdditionalCaption = "Additional";

	public static string DocumentCaption = "Document";

	public static string GutterMarginCaption = "Gutter&&Margin";

	public static string GroupBoxLineNumbersCaption_SyntaxSettingsDlg = "Line Numbers";

	public static string WordWrapCaption_SyntaxSettingsDlg = "Word Wrap";

	public static string HighlightUrlsCaption = "Highlight Urls";

	public static string DragAndDropCaption = "&Drag and drop text";

	public static string VertScrollBarCaption = "&Vertical scroll bar";

	public static string HorzScrollBarCaption = "&Horizontal scroll bar";

	public static string ForcedCaption = "Forced";

	public static string ShowGutterCaption = "Show Gutter";

	public static string ShowMarginCaption = "Show Margin";

	public static string GutterWidthCaption = "Gutter width:";

	public static string MarginPositionCaption = "Margin position:";

	public static string CheckBoxLineNumbersCaption_SyntaxSettingsDlg = "Show Line Numbers";

	public static string LineNumbersOnGutterCaption = "Display on Gutter";

	public static string FontCaption = "Font:";

	public static string SizeCaption = "&Size:";

	public static string DisplayItemsCaption = "&Display items:";

	public static string DescriptionCaption = "Description:";

	public static string ForeColorCaption = "Fore Color:";

	public static string BackColorCaption = "Back Color:";

	public static string FontAttributesCaption = "Attributes:";

	public static string BoldCaption = "Bold";

	public static string ItalicCaption = "Italic";

	public static string UnderlineCaption = "Underline";

	public static string SampleCaption = "Sample:";

	public static string SampleTextCaption = "AaBbYyZz";

	public static string NavigateOptionsCaption = "Navigate Options";

	public static string OutlineOptionsCaption = "Outline Options";

	public static string TabOptionsCaption = "Tab Options";

	public static string BeyondEolCaption = "Beyond Eol";

	public static string BeyondEofCaption = "Beyond Eof";

	public static string MoveOnRightButtonCaption = "Move on Right Button";

	public static string AllowOutliningCaption = "Allow outlining";

	public static string ShowHintsCaption = "Show Hints";

	public static string TabSizesCaption = "Tab Sizes:";

	public static string InsertSpacesCaption = "Insert s&paces";

	public static string KeepTabsCaption = "&Keep tabs";

	public static string OKCaption_SyntaxSettingsDlg = "OK";

	public static string CancelCaption_SyntaxSettingsDlg = "Cancel";

	public static string AddColorTheme = "Add Color Theme";

	public static string DeleteColorTheme = "Delete Color Theme";

	public static string WhiteSpaceCaption_SyntaxSettingsDlg = "White space";

	public static string LineModificatorCaption_SyntaxSettingsDlg = "Line modificator";

	public static string LineSeparatorCaption_SyntaxSettingsDlg = "Line separator";

	public static string KeyboardMappingSchemeCaption = "Keyboard mapping scheme:";

	public static string ShowCommandsCaption = "Show commands containing:";

	public static string ShortcutsCaption = "Shortcut(s) for selected command:";

	public static string UseShortcutCaption = "Use new shortcut in:";

	public static string ShortcutKeysCaption = "Press shortcut key(s):";

	public static string ShortcutCurrentlyUsedCaption = "Shortcut currently used by:";

	public static string SaveSchemeAsCaption = "Save As...";

	public static string DeleteSchemeCaption = "Delete";

	public static string RemoveShortcutCaption = "Remove";

	public static string AssignShortcutsCaption = "Assign";

	public static string MenuUndoCaption = "Undo";

	public static string MenuRedoCaption = "Redo";

	public static string MenuCutCaption = "Cut";

	public static string MenuCopyCaption = "Copy";

	public static string MenuPasteCaption = "Paste";

	public static string MenuDeleteCaption = "Delete";

	public static string MenuSelectAllCaption = "Select All";

	private StringConsts()
	{
	}

	public static void Localize(CultureInfo CultureInfo)
	{
		Thread.CurrentThread.CurrentUICulture = CultureInfo;
		ResourceManager resourceManager = new ResourceManager("QWhale.Common.Consts", Assembly.GetExecutingAssembly());
		try
		{
			LineNumberCaption = resourceManager.GetString("LineNumberCaption");
			NofMstr = resourceManager.GetString("NofMstr");
			LineNofMstr = resourceManager.GetString("LineNofMstr");
			LineStr = resourceManager.GetString("LineStr");
			PageNofMstr = resourceManager.GetString("PageNofMstr");
			PageStr = resourceManager.GetString("PageStr");
			DefaultHyperTextHint = "{0}\r\n" + resourceManager.GetString("DefaultHyperTextHint");
			DefaultDragMarginHint = resourceManager.GetString("DefaultDragMarginHint");
			EmptyOption = resourceManager.GetString("EmptyOption");
			PrintOptionsButtonText = resourceManager.GetString("PrintOptionsButtonText");
			InvalidTypeName = resourceManager.GetString("InvalidTypeName");
			InvalidPropName = resourceManager.GetString("InvalidPropName");
			InvalidTabStop = resourceManager.GetString("InvalidTabStop");
			InvalidMarginColumn = resourceManager.GetString("InvalidMarginColumn");
			InvalidReference = resourceManager.GetString("InvalidReference");
			InvalidRange = resourceManager.GetString("InvalidRange");
			StringNotFound = resourceManager.GetString("StringNotFound");
			SearchReachedStartPos = resourceManager.GetString("SearchReachedStartPos");
			DlgGotoCaption = resourceManager.GetString("DlgGoto");
			EnterNewLineCaption = resourceManager.GetString("laEnterNewLine_Goto");
			OKCaption_GotoDlg = resourceManager.GetString("btOK_Goto");
			CancelCaption_GotoDlg = resourceManager.GetString("btCancel_Goto");
			DlgPrintOptionsCaption = resourceManager.GetString("DlgPrintOptions");
			FileToPrintCaption = resourceManager.GetString("gbFileToPrint_PrintOptions");
			OptionsCaption = resourceManager.GetString("gbOptions_PrintOptions");
			FileNameCaption = resourceManager.GetString("laFileName_PrintOptions");
			PrintSelectionCaption = resourceManager.GetString("chbPrintSelection_PrintOptions");
			UseHeaderCaption = resourceManager.GetString("chbUseHeader_PrintOptions");
			UseFooterCaption = resourceManager.GetString("chbUseFooter_PrintOptions");
			LineNumbersCaption_PrintOptionsDlg = resourceManager.GetString("chbLineNumbers_PrintOptions");
			PageNumbersCaption = resourceManager.GetString("chbPageNumbers_PrintOptions");
			WordWrapCaption_PrintOptionsDlg = resourceManager.GetString("chbWordWrap_PrintOptions");
			UseSyntaxCaption = resourceManager.GetString("chbUseSyntax_PrintOptions");
			UseColorsCaption = resourceManager.GetString("chbUseColors_PrintOptions");
			DisplayProgressCaption = resourceManager.GetString("chbDisplayProgress_PrintOptions");
			OkCaption_PrintOptionsDlg = resourceManager.GetString("btOk_PrintOptions");
			CancelCaption_PrintOptionsDlg = resourceManager.GetString("btCancel_PrintOptions");
			DlgSearchCaption = resourceManager.GetString("DlgSearch");
			DlgReplaceCaption = resourceManager.GetString("DlgReplace");
			FindWhatCaption = resourceManager.GetString("laFindWhat_SearchDlg");
			ReplaceWithCaption = resourceManager.GetString("laReplaceWith_SearchDlg");
			MatchCaseCaption = resourceManager.GetString("chbMatchCase_SearchDlg");
			MatchWholeWordCaption = resourceManager.GetString("chbMatchWholeWord_SearchDlg");
			SearchHiddenTextCaption = resourceManager.GetString("chbSearchHiddenText_SearchDlg");
			SearchUpCaption = resourceManager.GetString("chbSearchUp_SearchDlg");
			UseRegularExpressionsCaption = resourceManager.GetString("chbUseRegularExpressions_SearchDlg");
			PromptOnReplaceCaption = resourceManager.GetString("chbPromptOnReplace_SearchDlg");
			SearchCaption = resourceManager.GetString("gbSearch_SearchDlg");
			FromCursorCaption = resourceManager.GetString("rbFromCursor_SearchDlg");
			EntireScopeCaption = resourceManager.GetString("rbEntireScope_SearchDlg");
			SelectionOnlyCaption = resourceManager.GetString("chbSelectionOnly_SearchDlg");
			FindNextCaption = resourceManager.GetString("btFindNext_SearchDlg");
			ReplaceCaption = resourceManager.GetString("btReplace_SearchDlg");
			ReplaceAllCaption = resourceManager.GetString("btReplaceAll_SearchDlg");
			MarkAllCaption = resourceManager.GetString("btMarkAll_SearchDlg");
			CloseCaption = resourceManager.GetString("btClose_SearchDlg");
			HelpCaption = resourceManager.GetString("btHelp_SearchDlg");
			SingleCharCaption = resourceManager.GetString("miSingleChar_SearchDlg");
			ZeroOrMoreCaption = resourceManager.GetString("miZeroOrMore_SearchDlg");
			OneOrMoreCaption = resourceManager.GetString("miOneOrMore_SearchDlg");
			BeginLineCaption = resourceManager.GetString("miBeginLine_SearchDlg");
			EndLineCaption = resourceManager.GetString("miEndLine_SearchDlg");
			OneCharInSetCaption = resourceManager.GetString("miOneCharInSet_SearchDlg");
			OneCharNotInSetCaption = resourceManager.GetString("miOneCharNotInSet_SearchDlg");
			OrCaption = resourceManager.GetString("miOr_SearchDlg");
			EscapeCaption = resourceManager.GetString("miEscape_SearchDlg");
			TagCaption = resourceManager.GetString("miTag_SearchDlg");
			FindWhatTextCaption = resourceManager.GetString("miFindWhatText_SearchDlg");
			TagExpressionCaption = resourceManager.GetString("miTagExpression_SearchDlg");
			ConfirmReplaceText = resourceManager.GetString("ConfirmReplaceText");
			OccurencesReplaced = resourceManager.GetString("OccurencesReplaced");
			ConfirmReplace = resourceManager.GetString("ConfirmReplace");
			IdentsName = resourceManager.GetString("IdentsName");
			NumbersName = resourceManager.GetString("NumbersName");
			ReswordsName = resourceManager.GetString("ReswordsName");
			CommentsName = resourceManager.GetString("CommentsName");
			StringsName = resourceManager.GetString("StringsName");
			WhiteSpaceName = resourceManager.GetString("WhiteSpaceName");
			DirectivesName = resourceManager.GetString("DirectivesName");
			XMLCommentsName = resourceManager.GetString("XMLCommentsName");
			HTMLTagsName = resourceManager.GetString("HTMLTagsName");
			LineNumbersName = resourceManager.GetString("LineNumbersName");
			HyperTextName = resourceManager.GetString("HyperTextName");
			MisspelledWordsName = resourceManager.GetString("MisspelledWordsName");
			SyntaxErrorsName = resourceManager.GetString("SyntaxErrorsName");
			CodeSnippetsName = resourceManager.GetString("CodeSnippetsName");
			SymbolsName = resourceManager.GetString("SymbolsName");
			BracesName = resourceManager.GetString("BracesName");
			OperatorsName = resourceManager.GetString("OperatorsName");
			FunctionsName = resourceManager.GetString("FunctionsName");
			ObjectsName = resourceManager.GetString("ObjectsName");
			NewObjectsName = resourceManager.GetString("NewObjectsName");
			LineModificatorChangedName = resourceManager.GetString("LineModificatorChangedName");
			LineModificatorSavedName = resourceManager.GetString("LineModificatorSavedName");
			LineSeparatorName = resourceManager.GetString("LineSeparatorName");
			WindowColorName = resourceManager.GetString("WindowColorName");
			BraceMatchingName = resourceManager.GetString("BraceMatchingName");
			GutterPenColorName = resourceManager.GetString("GutterPenColorName");
			CodeOutlinePenColorName = resourceManager.GetString("CodeOutlinePenColorName");
			ActiveSelectionName = resourceManager.GetString("ActiveSelectionName");
			InactiveSelectionName = resourceManager.GetString("InactiveSelectionName");
			IdentsInternalName = resourceManager.GetString("IdentsInternalName");
			NumbersInternalName = resourceManager.GetString("NumbersInternalName");
			ReswordsInternalName = resourceManager.GetString("ReswordsInternalName");
			CommentsInternalName = resourceManager.GetString("CommentsInternalName");
			StringsInternalName = resourceManager.GetString("StringsInternalName");
			WhiteSpaceInternalName = resourceManager.GetString("WhiteSpaceInternalName");
			DirectivesInternalName = resourceManager.GetString("DirectivesInternalName");
			XMLCommentsInternalName = resourceManager.GetString("XMLCommentsInternalName");
			HTMLTagsInternalName = resourceManager.GetString("HTMLTagsInternalName");
			LineNumbersInternalName = resourceManager.GetString("LineNumbersInternalName");
			HyperTextInternalName = resourceManager.GetString("HyperTextInternalName");
			MisspelledWordsInternalName = resourceManager.GetString("MisspelledWordsInternalName");
			SyntaxErrorsInternalName = resourceManager.GetString("SyntaxErrorsInternalName");
			CodeSnippetsInternalName = resourceManager.GetString("CodeSnippetsInternalName");
			SymbolsInternalName = resourceManager.GetString("SymbolsInternalName");
			BracesInternalName = resourceManager.GetString("BracesInternalName");
			OperatorsInternalName = resourceManager.GetString("OperatorsInternalName");
			FunctionsInternalName = resourceManager.GetString("FunctionsInternalName");
			ObjectsInternalName = resourceManager.GetString("ObjectsInternalName");
			NewObjectsInternalName = resourceManager.GetString("NewObjectsInternalName");
			LineModificatorChangedInternalName = resourceManager.GetString("LineModificatorChangedInternalName");
			LineModificatorSavedInternalName = resourceManager.GetString("LineModificatorSavedInternalName");
			LineSeparatorInternalName = resourceManager.GetString("LineSeparatorInternalName");
			WindowColorInternalName = resourceManager.GetString("WindowColorInternalName");
			BraceMatchingInternalName = resourceManager.GetString("BraceMatchingInternalName");
			GutterPenColorInternalName = resourceManager.GetString("GutterPenColorInternalName");
			CodeOutlinePenColorInternalName = resourceManager.GetString("CodeOutlinePenColorInternalName");
			ActiveSelectionInternalName = resourceManager.GetString("ActiveSelectionInternalName");
			InactiveSelectionInternalName = resourceManager.GetString("InactiveSelectionInternalName");
			DlgSyntaxSettingsCaption = resourceManager.GetString("DlgSyntaxSettings");
			PropertiesOptionsCaption = resourceManager.GetString("tvProperties_Options_SyntaxSettingDlg");
			PropertiesGeneralCaption = resourceManager.GetString("tvProperties_General_SyntaxSettingDlg");
			PropertiesFontsColorsCaption = resourceManager.GetString("tvProperties_FontsWithColors_SyntaxSettingDlg");
			PropertiesAdditionalCaption = resourceManager.GetString("tvProperties_Additional_SyntaxSettingDlg");
			PropertiesKeyboradCaption = resourceManager.GetString("tvProperties_Keyboard_SyntaxSettingDlg");
			GeneralCaption = resourceManager.GetString("tpGeneral_SyntaxSettingDlg");
			FontsAndColorsCaption = resourceManager.GetString("tpFontsAndColors_SyntaxSettingDlg");
			AdditionalCaption = resourceManager.GetString("tpAdditional_SyntaxSettingDlg");
			DocumentCaption = resourceManager.GetString("gbDocument_SyntaxSettingDlg");
			GutterMarginCaption = resourceManager.GetString("gbGutterMargin_SyntaxSettingDlg");
			GroupBoxLineNumbersCaption_SyntaxSettingsDlg = resourceManager.GetString("gbLineNumbers_SyntaxSettingDlg");
			WordWrapCaption_SyntaxSettingsDlg = resourceManager.GetString("chbWordWrap_SyntaxSettingDlg");
			HighlightUrlsCaption = resourceManager.GetString("chbHighlightUrls_SyntaxSettingDlg");
			DragAndDropCaption = resourceManager.GetString("chbDragAndDrop_SyntaxSettingDlg");
			VertScrollBarCaption = resourceManager.GetString("chbVertScrollBar_SyntaxSettingDlg");
			HorzScrollBarCaption = resourceManager.GetString("chbHorzScrollBar_SyntaxSettingDlg");
			ForcedCaption = resourceManager.GetString("chbForced_SyntaxSettingDlg");
			ShowGutterCaption = resourceManager.GetString("chbShowGutter_SyntaxSettingDlg");
			ShowMarginCaption = resourceManager.GetString("chbShowMargin_SyntaxSettingDlg");
			GutterWidthCaption = resourceManager.GetString("laGutterWidth_SyntaxSettingDlg");
			MarginPositionCaption = resourceManager.GetString("laMarginPosition_SyntaxSettingDlg");
			CheckBoxLineNumbersCaption_SyntaxSettingsDlg = resourceManager.GetString("chbLineNumbers_SyntaxSettingDlg");
			LineNumbersOnGutterCaption = resourceManager.GetString("chbLineNumbersOnGutter_SyntaxSettingDlg");
			FontCaption = resourceManager.GetString("laFont_SyntaxSettingDlg");
			SizeCaption = resourceManager.GetString("laSize_SyntaxSettingDlg");
			DisplayItemsCaption = resourceManager.GetString("laDisplayItems_SyntaxSettingDlg");
			DescriptionCaption = resourceManager.GetString("laDescription_SyntaxSettingDlg");
			ForeColorCaption = resourceManager.GetString("laForeColor_SyntaxSettingDlg");
			BackColorCaption = resourceManager.GetString("laBackColor_SyntaxSettingDlg");
			FontAttributesCaption = resourceManager.GetString("gbFontAttributes_SyntaxSettingDlg");
			BoldCaption = resourceManager.GetString("chbBold_SyntaxSettingDlg");
			ItalicCaption = resourceManager.GetString("chbItalic_SyntaxSettingDlg");
			UnderlineCaption = resourceManager.GetString("chbUnderline_SyntaxSettingDlg");
			SampleCaption = resourceManager.GetString("laSample_SyntaxSettingDlg");
			SampleTextCaption = resourceManager.GetString("laSampleText_SyntaxSettingDlg");
			NavigateOptionsCaption = resourceManager.GetString("gbNavigateOptions_SyntaxSettingDlg");
			OutlineOptionsCaption = resourceManager.GetString("gbOutlineOptions_SyntaxSettingDlg");
			TabOptionsCaption = resourceManager.GetString("gbTabOptions_SyntaxSettingDlg");
			BeyondEolCaption = resourceManager.GetString("chbBeyondEol_SyntaxSettingDlg");
			BeyondEofCaption = resourceManager.GetString("chbBeyondEof_SyntaxSettingDlg");
			MoveOnRightButtonCaption = resourceManager.GetString("chbMoveOnRightButton_SyntaxSettingDlg");
			AllowOutliningCaption = resourceManager.GetString("chbAllowOutlining_SyntaxSettingDlg");
			ShowHintsCaption = resourceManager.GetString("chbShowHints_SyntaxSettingDlg");
			TabSizesCaption = resourceManager.GetString("laTabSizes_SyntaxSettingDlg");
			InsertSpacesCaption = resourceManager.GetString("rbInsertSpaces_SyntaxSettingDlg");
			KeepTabsCaption = resourceManager.GetString("rbKeepTabs_SyntaxSettingDlg");
			OKCaption_SyntaxSettingsDlg = resourceManager.GetString("btOK_SyntaxSettingDlg");
			CancelCaption_SyntaxSettingsDlg = resourceManager.GetString("btCancel_SyntaxSettingDlg");
			AddColorTheme = resourceManager.GetString("btAddColorTheme_SyntaxSettingDlg");
			DeleteColorTheme = resourceManager.GetString("btDeleteColorTheme_SyntaxSettingDlg");
			WhiteSpaceCaption_SyntaxSettingsDlg = resourceManager.GetString("chbWhiteSpaceCaption_SyntaxSettingsDlg");
			LineModificatorCaption_SyntaxSettingsDlg = resourceManager.GetString("chbLineModificatorCaption_SyntaxSettingsDlg");
			LineSeparatorCaption_SyntaxSettingsDlg = resourceManager.GetString("chbLineSeparatorCaption_SyntaxSettingsDlg");
			KeyboardMappingSchemeCaption = resourceManager.GetString("laKeyboardMappingScheme_SyntaxSettingDlg");
			ShowCommandsCaption = resourceManager.GetString("laShowCommands_SyntaxSettingDlg");
			ShortcutsCaption = resourceManager.GetString("laShortcuts_SyntaxSettingDlg");
			UseShortcutCaption = resourceManager.GetString("laUseShortcut_SyntaxSettingDlg");
			ShortcutKeysCaption = resourceManager.GetString("laShortcutKeys_SyntaxSettingDlg");
			ShortcutCurrentlyUsedCaption = resourceManager.GetString("laShortcutCurrentlyUsed_SyntaxSettingDlg");
			SaveSchemeAsCaption = resourceManager.GetString("btSaveSchemeAs_SyntaxSettingDlg");
			DeleteSchemeCaption = resourceManager.GetString("btDeleteScheme_SyntaxSettingDlg");
			RemoveShortcutCaption = resourceManager.GetString("btRemoveShortcut_SyntaxSettingDlg");
			AssignShortcutsCaption = resourceManager.GetString("btAssignShortcuts_SyntaxSettingDlg");
			MenuUndoCaption = resourceManager.GetString("miUndo_DefaultMenu");
			MenuRedoCaption = resourceManager.GetString("miRedo_DefaultMenu");
			MenuCutCaption = resourceManager.GetString("miCut_DefaultMenu");
			MenuCopyCaption = resourceManager.GetString("miCopy_DefaultMenu");
			MenuPasteCaption = resourceManager.GetString("miPaste_DefaultMenu");
			MenuDeleteCaption = resourceManager.GetString("miDelete_DefaultMenu");
			MenuSelectAllCaption = resourceManager.GetString("miSelectAll_DefaultMenu");
		}
		catch
		{
		}
	}
}
