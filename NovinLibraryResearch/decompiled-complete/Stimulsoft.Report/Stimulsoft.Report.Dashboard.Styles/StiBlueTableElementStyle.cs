using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiBlueTableElementStyle : StiTableElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiBlueDashboardTableStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Blue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Blue;

	public override Color CellBackColor { get; set; } = Color.White;


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("eee");


	public override Color HeaderBackColor { get; set; } = StiColor.Get("3498db");


	public override Color HeaderForeColor { get; set; } = Color.White;


	public override Color FooterBackColor { get; set; } = StiColor.Get("3498db");


	public override Color FooterForeColor { get; set; } = Color.White;


	public override Color CellForeColor { get; set; } = StiColor.Get("323a45");


	public override Color SelectedCellBackColor { get; set; } = StiColorUtils.Light("3498db", (byte)30);


	public override Color SelectedCellForeColor { get; set; } = Color.White;


	public override Color HotHeaderBackColor { get; set; } = StiColorUtils.Dark("3498db", (byte)20);

}
