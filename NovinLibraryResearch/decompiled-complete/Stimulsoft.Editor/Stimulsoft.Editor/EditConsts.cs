using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class EditConsts
{
	public static OutlineOptions DefaultOutlineOptions = OutlineOptions.DrawLines | OutlineOptions.DrawButtons | OutlineOptions.ShowHints;

	public static GutterOptions DefaultGutterOptions = GutterOptions.PaintBookMarks | GutterOptions.PaintLineModificators;

	public static CodeCompletionFlags DefaultCodeCompletionFlags = CodeCompletionFlags.CloseOnEscape | CodeCompletionFlags.AcceptOnEnter | CodeCompletionFlags.CloseOnMouseLeave | CodeCompletionFlags.CloseOnLostFocus | CodeCompletionFlags.AcceptOnDblClick | CodeCompletionFlags.FeetToScreen | CodeCompletionFlags.AcceptOnDelimiter | CodeCompletionFlags.AcceptOnTab;

	public static PrintOptions DefaultPrintOptions = PrintOptions.LineNumbers | PrintOptions.PageNumbers | PrintOptions.WordWrap | PrintOptions.UseColors | PrintOptions.UseSyntax | PrintOptions.UseHeader | PrintOptions.UseFooter | PrintOptions.DisplayProgress;

	public static SelectionOptions DefaultSelectionOptions = SelectionOptions.OverwriteBlocks | SelectionOptions.SmartFormat | SelectionOptions.ClearOnDrag | SelectionOptions.CopyLineWhenEmpty;

	public static NavigateOptions DefaultNavigateOptions = NavigateOptions.UpAtLineBegin | NavigateOptions.DownAtLineEnd;

	public static IndentOptions DefaultIndentOptions = IndentOptions.AutoIndent | IndentOptions.SmartIndent;

	public static UndoOptions DefaultUndoOptions = UndoOptions.AllowUndo | UndoOptions.GroupUndo | UndoOptions.UndoAfterSave;

	public static SearchOptions DefaultSearchOptions = SearchOptions.FindTextAtCursor | SearchOptions.FindSelectedText | SearchOptions.CycledSearch;

	public static LineStyleOptions DefaultLineStyleOptions = LineStyleOptions.InvertColors;

	public static RulerOptions DefaultRulerOptions = RulerOptions.Discrete | RulerOptions.AllowDrag | RulerOptions.DisplayDragLine;

	public static AllowedSelectionMode DefaultSelectionMode = AllowedSelectionMode.Stream | AllowedSelectionMode.Block;

	public static ScrollingOptions DefaultScrollingOptions = ScrollingOptions.SmoothScroll | ScrollingOptions.UseScrollDelta | ScrollingOptions.SystemScrollbars;

	public static Color DefaultBracesForeColor = Color.Black;

	public static Color DefaultBracesBackColor = Color.Empty;

	public static Color DefaultArrowBackColor = Color.Silver;

	public static Color DefaultArrowForeColor = Color.Black;

	public static Color DefaultLineBookmarksColor = Color.Red;

	public static Color DefaultLineNumbersForeColor = Color.Teal;

	public static Color DefaultLineNumbersBackColor = SystemColors.Window;

	public static Color DefaultLineStyleForeColor = SystemColors.WindowText;

	public static Color DefaultLineStyleBackColor = SystemColors.Window;

	public static Color DefaultPrintHeaderForeColor = Color.Black;

	public static Color DefaultTransparentColor = Color.Fuchsia;

	public static Color DefaultUrlForeColor = Color.Blue;

	public static Color DefaultGutterBackColor = SystemColors.Control;

	public static Color DefaultGutterForeColor = Color.Gray;

	public static Color DefaultDisabledForeColor = SystemColors.GrayText;

	public static Color DefaultInfoBackColor = SystemColors.Info;

	public static Color DefaultLineSeparatorColor = SystemColors.ScrollBar;

	public static Color DefaultLineSeparatorLineColor = SystemColors.ControlText;

	public static Color DefaultContentDividerColor = Color.Gray;

	public static Color DefaultHighlightForeColor = SystemColors.HighlightText;

	public static Color DefaultHighlightBackColor = SystemColors.Highlight;

	public static Color DefaultInactiveHighlightForeColor = SystemColors.InactiveCaptionText;

	public static Color DefaultInactiveHighlightBackColor = SystemColors.InactiveCaption;

	public static Color DefaultSelectionBorderColor = SystemColors.InactiveCaption;

	public static Color DefaultSelectionDragColor = Color.Chocolate;

	public static Color DefaultMarginForeColor = Color.Gray;

	public static Color DefaultMarginColumnForeColor = Color.LightGray;

	public static Color DefaultColumnsIndentForeColor = SystemColors.ControlLight;

	public static Color DefaultOutlineForeColor = Color.DarkGray;

	public static Color DefaultSpellForeColor = Color.Red;

	public static Color DefaultHeaderFontColor = Color.LightGray;

	public static Color DefaultRulerBackColor = Color.FromArgb(216, 231, 252);

	public static Color DefaultRulerIndentBackColor = Color.FromArgb(158, 190, 245);

	public static Color DefaultLineModificatorChangedColor = Color.Yellow;

	public static Color DefaultLineModificatorSavedColor = Color.Lime;

	public static Color DefaultSyntaxErrorForeColor = Color.Red;

	public static Color DefaultSnippetBackColor = Color.LightSteelBlue;

	public static Color DefaultPageBackColor = SystemColors.ControlDark;

	public static Color DefaultPageBorderColor = SystemColors.WindowFrame;

	public static Color DefaultUserMarginBackColor = SystemColors.Window;

	public static Color DefaultUserMarginForeColor = SystemColors.ControlText;

	public static Color DefaultReswordForeColor = Color.Blue;

	public static Color DefaultCommentsForeColor = Color.Green;

	public static Color DefaultStringsForeColor = Color.Maroon;

	public static Color DefaultDirectivesForeColor = Color.DarkGreen;

	public static Color DefaultXmlCommentsForeColor = Color.Gray;

	public static Color DefaultHtmlTagsForeColor = Color.Maroon;

	public static Color DefaultHyperTextForeColor = Color.Blue;

	public static Color DefaultWhiteSpaceForeColor = Color.Teal;

	public static Color DefaultCodeSnippetForeColor = Color.Black;

	public static Color DefaultCodeSnippetBackColor = Color.FromArgb(180, 228, 180);

	public static FontStyle DefaultBracesFontStyle = (FontStyle)1;

	public static FontStyle DefaultHeaderFontStyle = (FontStyle)0;

	public static FontStyle DefaultUrlFontStyle = (FontStyle)4;

	public static FontStyle DefaultCommentsFontStyle = (FontStyle)2;

	public static char[] DefaultOpenBraces = new char[3] { '(', '[', '{' };

	public static char[] DefaultClosingBraces = new char[3] { ')', ']', '}' };

	public static char DefaultCaretSymbol = '|';

	public static string DefaultOutlineText = "...";

	public static string ColumnSeparator = "|";

	public static string DefaultDelimiters = ";.,:'\"{}[]()?!@#$%^&*-+=|\\/<>";

	public static string DefaultUrlChars = "_.:/@-?=~&;%+";

	public static string DefaultSpellDelimiters = DefaultDelimiters;

	public static string DefaultAutoCorrectDelimiters = " .,;:\r\t";

	public static string MailTo = "MAILTO:";

	public static string WWW = "WWW.";

	public static string HTTPProtocol = "HTTP://";

	public static string FTPProtocol = "FTP://";

	public static string GopherProtocol = "GOPHER://";

	public static string FileProtocol = "FILE://";

	public static string PageTag = "\\[page]";

	public static string PagesTag = "\\[pages]";

	public static string DateTag = "\\[date]";

	public static string TimeTag = "\\[time]";

	public static string UserTag = "\\[username]";

	public static string HeaderFontName = "Times New";

	public static int DefaultPageHorzOffset = 64;

	public static int DefaultPageVertOffset = 24;

	public static int DefaultPageMargin = 100;

	public static int DefaultPageMarginSize = 24;

	public static string DefaultUserMarginText = "\\[chars] chars";

	public static string LineTag = "\\[line]";

	public static string CharsTag = "\\[chars]";

	public static char TabSymbol = '→';

	public static char SpaceSymbol = '·';

	public static char EolSymbol = '¶';

	public static char EofSymbol = '_';

	public static char WordWrapSymbol = '…';

	public static string DottedText = "...";

	public static string DefaultCodeCompletionChars = ".(";

	public static string DefaultSnippetPattern = "[a-zA-Z_][a-zA-Z0-9_]*";

	public static string DefaultSnippetSelectedPattern = "selected";

	public static string DefaultSnippetEndPattern = "end";

	public static int DefaultDropDownCount = 8;

	public static int MaxDropDownCount = 10;

	public static int DefaultMinListBoxWidth = 50;

	public static int DefaultMinListBoxHeight = 50;

	public static int MaxHintWindowWidth = Screen.PrimaryScreen.Bounds.Width * 2 / 3;

	public static int MaxHintWindowCount = 20;

	public static int DefaultHintOffsetX = 8;

	public static int DefaultHintOffsetY = 4;

	public static int DefaultSpacesInTab = 4;

	public static float DefaultCodeCompletionFontSize = 8.5f;

	public static string DefaultCodeCompletionFont = "Tahoma";

	public static int DefaultRulerHeight = 28;

	public static int DefaultRulerIndentSize = 2;

	public static int DefaultRulerHitWidth = 4;

	public static int DefaultRulerMinWidth = 40;

	public static RulerUnits DefaultRulerUnits = RulerUnits.Inches;

	public static string DefaultRulerFontName = "Arial";

	public static float DefaultRulerFontSize = 7f;

	public const int DefaultPrintHeaderOffsetX = 24;

	public const int DefaultPrintHeaderOffsetY = 8;

	public const int MinPageIndent = 4;

	public const EditBorderStyle DefaultBorderStyle = EditBorderStyle.Fixed3D;

	public static int DefaultCaretWidth = 2;

	public static int DefaultHorzScrollSize = 255;

	public static int DefaultGutterWidth = 16;

	public static int DefaultUserMarginWidth = 80;

	public static int DefaultMarginPosition = 80;

	public static int DefaultColumnMarginPosition = 16;

	public static int DefaultMarginHitWidth = 4;

	public static int DefaultTabStop = 4;

	public static int DefaultParserDelta = 128;

	public static int MaxPaintChar = 1024;

	public static int DefaultLineModificatorWidth = 4;

	public static int DefaltScrollSplitterSize = 6;

	public static int DefaultScrollDeltaRatio = 5;

	public static int DefaultCompletionStartDelay = 100;

	public static int MaxFontSize = 1024;

	public static int DefaultSelDelay = 32;

	public static int DefaultDragDelay = 500;

	public static int DefaultOutlineDelay = 1000;

	public static int DefaultBracesDelay = 2000;

	public static int DefaultHideHintDelay = 2500;

	public static int DefaultWrapDelta = 128;

	public static int DefaultCollasedImageWidth = 8;

	public static int DefaultColumnSeparator = 2;

	public static int DefaultRowSeparator = 2;

	public static int DefaultLineNumbersStart = 1;

	public static int DefaultLineNumbersIndent = 2;

	public static int DefaultOutliningIndent = 1;

	public static int DefaultBookMarkImageIndex = 10;

	public static int DefaultWrapImageIndex = 13;

	public const int WheelDelta = 120;

	public static Keys[] NavKeys;

	static EditConsts()
	{
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		Keys[] array = new Keys[9];
		RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		NavKeys = (Keys[])(object)array;
	}
}
