using System;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Blocks;

public interface IStiBlocklyEditorWindow
{
	string Xml { get; }

	object EventValue { get; set; }

	IStiReport Report { get; set; }

	event EventHandler SwitchToCode;

	bool? ShowDialog(IntPtr? intPtr, PointD? centerOwner);
}
