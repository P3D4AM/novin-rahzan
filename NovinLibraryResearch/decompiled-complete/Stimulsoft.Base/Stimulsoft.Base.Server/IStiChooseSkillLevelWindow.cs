using System;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Server;

public interface IStiChooseSkillLevelWindow
{
	StiDesignerSpecification? DesignerSpecification { get; set; }

	bool? ShowDialog();

	bool? ShowDialog(IntPtr intPtr, PointD centerOwner);

	bool? ShowDialog(IntPtr intPtr);
}
