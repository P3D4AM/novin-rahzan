using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiGreenTableElementStyle : StiTableElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiGreenDashboardTableStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Green");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Green;

	public override Color CellBackColor { get; set; } = Color.White;


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("eee");


	public override Color SelectedCellBackColor { get; set; } = StiColorUtils.Light("70ad47", (byte)30);


	public override Color HeaderBackColor { get; set; } = StiColor.Get("70ad47");


	public override Color HotHeaderBackColor { get; set; } = StiColorUtils.Light("70ad47", (byte)30);


	public override Color FooterBackColor { get; set; } = StiColor.Get("70ad47");


	public override Color FooterForeColor { get; set; } = Color.White;

}
