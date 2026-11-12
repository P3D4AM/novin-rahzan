using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class DocumentSerializationXml
{
	public static string Documents = "documents";

	public static string DockContainer = "dockcontainer";

	public static string BarContainer = "barcontainer";

	public static string Orientation = "orientation";

	public static string Width = "w";

	public static string Height = "h";

	public static string DockSite = "docksite";

	public static string DockingSide = "dockingside";

	public static string DockSiteSize = "size";

	public static string Version = "version";

	public static string OriginalDockSiteSize = "originaldocksitesize";

	public static DocumentBaseContainer CreateDocument(string xmlName)
	{
		if (xmlName == DockContainer)
		{
			return new DocumentDockContainer();
		}
		if (xmlName == BarContainer)
		{
			return new DocumentBarContainer();
		}
		throw new InvalidOperationException("Document type not recognized: " + xmlName);
	}
}
