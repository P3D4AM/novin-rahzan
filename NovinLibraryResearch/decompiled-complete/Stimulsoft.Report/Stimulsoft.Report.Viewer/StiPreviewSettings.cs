using System;
using System.ComponentModel;

namespace Stimulsoft.Report.Viewer;

[Flags]
public enum StiPreviewSettings
{
	All = 0xFFFFFFF,
	None = 0,
	Default = 0xF7FE1FF,
	PageViewMode = 1,
	VertScrollBar = 2,
	HorScrollBar = 4,
	StatusBar = 8,
	Print = 0x10,
	Open = 0x20,
	Save = 0x40,
	Parameters = 0x80,
	SendEMail = 0x100,
	PageNew = 0x200,
	PageDelete = 0x400,
	PageDesign = 0x800,
	PageSize = 0x1000,
	Resources = 0x2000,
	[Obsolete("Value 'HandTool' is obsolete.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	HandTool = 0x8000,
	[Obsolete("Please use 'Editor' value instead 'EditorTool' value.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	EditorTool = 0x10000,
	[Obsolete("Please use 'Find' value instead 'FindTool' value.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	FindTool = 0x20000,
	Editor = 0x10000,
	Find = 0x20000,
	Zoom = 0x40000,
	PageControl = 0x80000,
	Bookmarks = 0x100000,
	Thumbs = 0x200000,
	ContextMenu = 0x400000,
	Close = 0x800000,
	Toolbar = 0x1000000
}
