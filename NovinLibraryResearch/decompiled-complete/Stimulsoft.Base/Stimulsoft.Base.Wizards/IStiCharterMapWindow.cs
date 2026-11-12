using System;

namespace Stimulsoft.Base.Wizards;

public interface IStiCharterMapWindow
{
	string Result { get; }

	bool? ShowDialog(IntPtr intPtr);
}
