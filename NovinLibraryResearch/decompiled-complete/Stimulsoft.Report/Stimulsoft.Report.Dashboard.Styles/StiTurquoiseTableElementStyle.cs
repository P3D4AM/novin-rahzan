using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiTurquoiseTableElementStyle : StiTableElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiTurquoiseDashboardTableStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Turquoise");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Turquoise;

	public override Color CellBackColor { get; set; } = Color.White;


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("eee");


	public override Color SelectedCellBackColor { get; set; } = StiColorUtils.Light("2ec6c8", (byte)30);


	public override Color HeaderBackColor { get; set; } = StiColor.Get("2ec6c8");


	public override Color HotHeaderBackColor { get; set; } = StiColorUtils.Light("2ec6c8", (byte)30);


	public override Color FooterBackColor { get; set; } = StiColor.Get("2ec6c8");


	public override Color FooterForeColor { get; set; } = Color.White;

}
