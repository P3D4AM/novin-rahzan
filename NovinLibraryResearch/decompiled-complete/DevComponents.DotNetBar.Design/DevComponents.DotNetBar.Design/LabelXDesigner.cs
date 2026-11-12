using System;
using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace DevComponents.DotNetBar.Design;

public class LabelXDesigner : ControlDesigner
{
	public override IList SnapLines
	{
		get
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Invalid comparison between Unknown and I4
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Invalid comparison between Unknown and I4
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Expected O, but got Unknown
			LabelX labelX = ((ControlDesigner)this).Control as LabelX;
			IList snapLines = ((ControlDesigner)this).SnapLines;
			int num = (int)Math.Floor(((Control)labelX).Font.Size * (float)((Control)labelX).Font.FontFamily.GetCellDescent(((Control)labelX).Font.Style) / (float)((Control)labelX).Font.FontFamily.GetEmHeight(((Control)labelX).Font.Style));
			num = (((int)labelX.TextLineAlignment == 1) ? ((int)Math.Floor((double)(((Control)labelX).Height - ((Control)labelX).Font.Height) / 2.0) + ((Control)labelX).Font.Height - num) : (((int)labelX.TextLineAlignment != 2) ? (((Control)labelX).Font.Height - num) : (((Control)labelX).Height - num)));
			snapLines.Add((object?)new SnapLine((SnapLineType)6, num, (SnapLinePriority)2));
			return snapLines;
		}
	}
}
