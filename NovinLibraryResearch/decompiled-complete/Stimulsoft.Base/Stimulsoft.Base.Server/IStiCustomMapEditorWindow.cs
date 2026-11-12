using System;

namespace Stimulsoft.Base.Server;

public interface IStiCustomMapEditorWindow
{
	string ResultJson { get; }

	bool? ShowDialog(IntPtr intPtr);
}
