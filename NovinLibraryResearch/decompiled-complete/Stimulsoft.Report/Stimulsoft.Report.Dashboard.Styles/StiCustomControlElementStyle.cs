namespace Stimulsoft.Report.Dashboard.Styles;

public class StiCustomControlElementStyle : StiControlElementStyle
{
	private string styleName;

	public override StiComponentId ComponentId => StiComponentId.StiCustomDashboardControlStyle;

	public override string LocalizedName => styleName;

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Custom;

	public StiCustomControlElementStyle(StiDialogStyle style)
	{
		base.Font = style.Font;
		styleName = style.Name;
		BackColor = style.BackColor;
		ForeColor = style.ForeColor;
		GlyphColor = style.GlyphColor;
		HotBackColor = style.HotBackColor;
		HotForeColor = style.HotForeColor;
		HotGlyphColor = style.HotGlyphColor;
		HotSelectedBackColor = style.HotSelectedBackColor;
		HotSelectedForeColor = style.HotSelectedForeColor;
		HotSelectedGlyphColor = style.HotSelectedGlyphColor;
		SelectedBackColor = style.SelectedBackColor;
		SelectedForeColor = style.SelectedForeColor;
		SelectedGlyphColor = style.SelectedGlyphColor;
		SeparatorColor = style.SeparatorColor;
	}
}
