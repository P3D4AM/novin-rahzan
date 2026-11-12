using System.Drawing;

namespace Stimulsoft.Editor;

internal class LexStyleItems
{
	public static LexStyleItem[] Items = InitStyles();

	public static LexStyleItem[] InitStyles()
	{
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		return new LexStyleItem[27]
		{
			new LexStyleItem(StringConsts.SymbolsName, StringConsts.SymbolsInternalName),
			new LexStyleItem(StringConsts.BracesName, StringConsts.BracesInternalName),
			new LexStyleItem(StringConsts.OperatorsName, StringConsts.OperatorsInternalName),
			new LexStyleItem(StringConsts.FunctionsName, StringConsts.FunctionsInternalName),
			new LexStyleItem(StringConsts.ObjectsName, StringConsts.ObjectsInternalName),
			new LexStyleItem(StringConsts.NewObjectsName, StringConsts.NewObjectsInternalName),
			new LexStyleItem(StringConsts.IdentsName, StringConsts.IdentsInternalName),
			new LexStyleItem(StringConsts.NumbersName, StringConsts.NumbersInternalName),
			new LexStyleItem(StringConsts.ReswordsName, StringConsts.ReswordsInternalName, EditConsts.DefaultReswordForeColor),
			new LexStyleItem(StringConsts.CommentsName, StringConsts.CommentsInternalName, EditConsts.DefaultCommentsForeColor, Color.Empty, (FontStyle)0, plainText: true),
			new LexStyleItem(StringConsts.StringsName, StringConsts.StringsInternalName, EditConsts.DefaultStringsForeColor, Color.Empty, (FontStyle)0, plainText: true),
			new LexStyleItem(StringConsts.DirectivesName, StringConsts.DirectivesInternalName, EditConsts.DefaultDirectivesForeColor),
			new LexStyleItem(StringConsts.XMLCommentsName, StringConsts.XMLCommentsInternalName, EditConsts.DefaultXmlCommentsForeColor, Color.Empty, (FontStyle)0, plainText: true),
			new LexStyleItem(StringConsts.HTMLTagsName, StringConsts.HTMLTagsInternalName, EditConsts.DefaultHtmlTagsForeColor),
			new LexStyleItem(StringConsts.WhiteSpaceName, StringConsts.WhiteSpaceInternalName),
			new LexStyleItem(StringConsts.LineNumbersName, StringConsts.LineNumbersInternalName, EditConsts.DefaultLineNumbersForeColor),
			new LexStyleItem(StringConsts.HyperTextName, StringConsts.HyperTextInternalName, EditConsts.DefaultHyperTextForeColor, Color.Empty, EditConsts.DefaultUrlFontStyle, plainText: false),
			new LexStyleItem(StringConsts.MisspelledWordsName, StringConsts.MisspelledWordsInternalName, EditConsts.DefaultSpellForeColor),
			new LexStyleItem(StringConsts.WindowColorName, StringConsts.WindowColorInternalName, Color.Empty, Consts.DefaultControlBackColor),
			new LexStyleItem(StringConsts.BraceMatchingName, StringConsts.BraceMatchingInternalName, EditConsts.DefaultBracesForeColor, EditConsts.DefaultBracesBackColor),
			new LexStyleItem(StringConsts.GutterPenColorName, StringConsts.GutterPenColorInternalName, EditConsts.DefaultGutterForeColor, EditConsts.DefaultGutterBackColor),
			new LexStyleItem(StringConsts.LineModificatorChangedName, StringConsts.LineModificatorChangedInternalName, EditConsts.DefaultLineModificatorChangedColor, Color.Empty),
			new LexStyleItem(StringConsts.LineModificatorSavedName, StringConsts.LineModificatorSavedInternalName, EditConsts.DefaultLineModificatorSavedColor, Color.Empty),
			new LexStyleItem(StringConsts.LineSeparatorName, StringConsts.LineSeparatorInternalName, EditConsts.DefaultLineSeparatorLineColor, Color.Empty),
			new LexStyleItem(StringConsts.CodeOutlinePenColorName, StringConsts.CodeOutlinePenColorInternalName, EditConsts.DefaultOutlineForeColor, Color.Empty),
			new LexStyleItem(StringConsts.ActiveSelectionName, StringConsts.ActiveSelectionInternalName, EditConsts.DefaultHighlightForeColor, EditConsts.DefaultHighlightBackColor),
			new LexStyleItem(StringConsts.InactiveSelectionName, StringConsts.InactiveSelectionInternalName, EditConsts.DefaultInactiveHighlightForeColor, EditConsts.DefaultInactiveHighlightBackColor)
		};
	}

	public static void Localize()
	{
		Items = InitStyles();
	}
}
