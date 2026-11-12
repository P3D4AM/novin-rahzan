using System.Drawing;

namespace Stimulsoft.Editor;

public class SyntaxConsts
{
	public static string DefaultLexSchemeVersion = "1.1";

	public static Color DefaultTransparentColor = Color.Magenta;

	public const string XmlCommentStart = "///";

	public const string XmlVbCommentStart = "'''";

	public static string OutlineText = "Outline_Text";

	public static string Description = "Description";

	public static SyntaxOptions DefaultNetSyntaxOptions = SyntaxOptions.Outline | SyntaxOptions.SmartIndent | SyntaxOptions.CodeCompletion | SyntaxOptions.SyntaxErrors;

	public static string DeclarationScope = "Declaration_Scope";

	public static string DefinitionScope = "Definition_Scope";

	public static string DefinitionScopeEndExpected = "Definition_Scope_End_Expected";

	public static string DefinitionScopeEnd = "Definition_Scope_End";

	public static string BlockScope = "Block_Scope";

	public static string SuppressReparsingScope = "SuppressReparsing_Scope";

	public const string Constructor = ".ctor";

	public const string ElementAccess = ".ElementAccess";

	public const string PublicModifier = "public";

	public const string PrivateModifier = "private";

	public const string ProtectedModifier = "protected";

	public const string InternalModifier = "internal";

	public const string StaticModifier = "static";

	public const string SharedModifier = "shared";

	public const string PartialModifier = "partial";

	public const string AbstractModifier = "abstract";

	public const string VirtualModifier = "virtual";

	public const string RefParameter = "ref";

	public const string OutParameter = "out";

	public static string OutlineCommentText = "/**/";

	public static string OutlineUsingText = "using";

	public static string OutlineImportText = "import";

	public static string OutlineImportsText = "imports";

	public static string ClassDataType = "class";

	public static string EnumDataType = "enum";

	public static string InterfaceDataType = "interface";

	public static string StructDataType = "struct";

	public static string TypeDataType = "type";

	public static string NamespaceDataType = "namespace";

	public static string SummaryDescription = "Summary";

	public static string StructureResWord = "Structure";

	public static string NameTag = "name";

	public static string ExpressionTag = "Expression";

	public static string TypeTag = "Type";

	public static string DefaultNetCodeCompletionChars = ".(";

	public static string ExtendedNetCodeCompletionChars = ".(abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

	public static string DefaultNetCodeCompletionStopChars = ")";

	public static string DefaultNetAutoIndentChars = "\r\n";

	public static string DefaultCsCodeCompletionChars = ".(<>[ ";

	public static string DefaultCsCodeCompletionStopChars = ")]";

	public static string DefaultCsAutoIndentChars = ":{\r\n";

	public static string DefaultCsSmartFormatChars = "}";

	public static string DefaultVbCodeCompletionChars = ".(=,< ";

	public static string DefaultBoldTag = "<b>";

	public static string DefaultBoldEndTag = "</b>";

	public static string DefaultBrTag = "<br>";

	public static string DefaultSnippetDelimiter = "$";

	public static string DefaultSnippetPathSeparator = ">";

	public static int DefaultHintDelay = 500;

	public static int DefaultCompletionDelay = 100;
}
