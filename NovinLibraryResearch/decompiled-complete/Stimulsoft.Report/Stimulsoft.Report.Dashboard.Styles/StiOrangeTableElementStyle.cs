using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiOrangeTableElementStyle : StiTableElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiOrangeDashboardTableStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Orange");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Orange;

	public override Color CellBackColor { get; set; } = Color.White;


	public override Color AlternatingCellBackColor { get; set; } = StiColor.Get("eee");


	public override Color SelectedCellBackColor { get; set; } = StiColorUtils.Light("ed7d31", (byte)30);


	public override Color HeaderBackColor { get; set; } = StiColor.Get("ed7d31");


	public override Color HotHeaderBackColor { get; set; } = StiColorUtils.Light("ed7d31", (byte)30);


	public override Color FooterBackColor { get; set; } = StiColor.Get("ed7d31");


	public override Color FooterForeColor { get; set; } = Color.White;

}
