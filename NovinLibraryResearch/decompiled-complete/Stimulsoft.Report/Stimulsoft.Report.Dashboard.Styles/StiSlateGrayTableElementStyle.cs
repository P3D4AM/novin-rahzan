using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiSlateGrayTableElementStyle : StiTableElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiSlateGrayDashboardTableStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "SlateGray");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.SlateGray;

	public override Color CellBackColor { get; set; } = StiColor.Get("475b6f");


	public override Color CellForeColor { get; set; } = StiColor.Get("ddd");


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("33475b");


	public override Color AlternatingCellForeColor { get; set; } = StiColor.Get("ddd");


	public override Color HeaderBackColor { get; set; } = StiColor.Get("33475b");


	public override Color HeaderForeColor { get; set; } = StiColor.Get("ddd");


	public override Color FooterBackColor { get; set; } = StiColor.Get("33475b");


	public override Color FooterForeColor { get; set; } = StiColor.Get("ddd");


	public override Color SelectedCellBackColor { get; set; } = StiColorUtils.Light("33475b", (byte)50);


	public override Color SelectedCellForeColor { get; set; } = StiColor.Get("eee");


	public override Color HotHeaderBackColor { get; set; } = StiColorUtils.Light("33475b", (byte)30);


	public override Color LineColor { get; set; } = StiColor.Get("c0c0c0");


	public override Color BackColor { get; set; } = StiColor.Get("33475b");

}
