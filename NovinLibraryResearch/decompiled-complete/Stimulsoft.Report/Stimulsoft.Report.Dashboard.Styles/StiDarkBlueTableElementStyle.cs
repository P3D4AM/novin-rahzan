using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkBlueTableElementStyle : StiTableElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkBlueDashboardTableStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkBlue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkBlue;

	public override Color CellBackColor { get; set; } = StiColor.Get("1f4265");


	public override Color CellForeColor { get; set; } = StiColor.Get("ddd");


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("0a325a");


	public override Color AlternatingCellForeColor { get; set; } = StiColor.Get("ddd");


	public override Color HeaderBackColor { get; set; } = StiColor.Get("0a325a");


	public override Color HeaderForeColor { get; set; } = StiColor.Get("ddd");


	public override Color FooterBackColor { get; set; } = StiColor.Get("0a325a");


	public override Color FooterForeColor { get; set; } = StiColor.Get("ddd");


	public override Color SelectedCellBackColor { get; set; } = StiColorUtils.Light("0a325a", (byte)50);


	public override Color SelectedCellForeColor { get; set; } = StiColor.Get("eee");


	public override Color HotHeaderBackColor { get; set; } = StiColorUtils.Light("0a325a", (byte)30);


	public override Color LineColor => StiColor.Get("758696");

	public override Color BackColor { get; set; } = StiColor.Get("0a325a");

}
