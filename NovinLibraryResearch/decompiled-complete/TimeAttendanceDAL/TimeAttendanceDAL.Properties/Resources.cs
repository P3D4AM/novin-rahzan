using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace TimeAttendanceDAL.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("TimeAttendanceDAL.Properties.Resources", typeof(Resources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static Icon Buffer => (Icon)ResourceManager.GetObject("Buffer", resourceCulture);

	internal static Icon cancel => (Icon)ResourceManager.GetObject("cancel", resourceCulture);

	internal static string Create_Functions => ResourceManager.GetString("Create_Functions", resourceCulture);

	internal static string Create_StoresProcedures => ResourceManager.GetString("Create_StoresProcedures", resourceCulture);

	internal static string Create_Tables => ResourceManager.GetString("Create_Tables", resourceCulture);

	internal static string Create_Triggers => ResourceManager.GetString("Create_Triggers", resourceCulture);

	internal static string Create_Views => ResourceManager.GetString("Create_Views", resourceCulture);

	internal static string CreateTaghvim => ResourceManager.GetString("CreateTaghvim", resourceCulture);

	internal static string DataInTables => ResourceManager.GetString("DataInTables", resourceCulture);

	internal static string Drop_Fields => ResourceManager.GetString("Drop_Fields", resourceCulture);

	internal static Icon EndSync => (Icon)ResourceManager.GetObject("EndSync", resourceCulture);

	internal static Icon error => (Icon)ResourceManager.GetObject("error", resourceCulture);

	internal static Icon exclamtion => (Icon)ResourceManager.GetObject("exclamtion", resourceCulture);

	internal static Bitmap Grip => (Bitmap)ResourceManager.GetObject("Grip", resourceCulture);

	internal static Icon information => (Icon)ResourceManager.GetObject("information", resourceCulture);

	internal static byte[] Internet_Update => (byte[])ResourceManager.GetObject("Internet_Update", resourceCulture);

	internal static Icon loading => (Icon)ResourceManager.GetObject("loading", resourceCulture);

	internal static Icon ok => (Icon)ResourceManager.GetObject("ok", resourceCulture);

	internal static Icon question => (Icon)ResourceManager.GetObject("question", resourceCulture);

	internal static Bitmap question_mark => (Bitmap)ResourceManager.GetObject("question_mark", resourceCulture);

	internal static string RemoveNoNeedTables => ResourceManager.GetString("RemoveNoNeedTables", resourceCulture);

	internal static Bitmap ServerStatus_Delivered => (Bitmap)ResourceManager.GetObject("ServerStatus_Delivered", resourceCulture);

	internal static Bitmap ServerStatus_DeliveryFailed => (Bitmap)ResourceManager.GetObject("ServerStatus_DeliveryFailed", resourceCulture);

	internal static Bitmap ServerStatus_None => (Bitmap)ResourceManager.GetObject("ServerStatus_None", resourceCulture);

	internal static Bitmap ServerStatus_SendError => (Bitmap)ResourceManager.GetObject("ServerStatus_SendError", resourceCulture);

	internal static Bitmap ServerStatus_Sent => (Bitmap)ResourceManager.GetObject("ServerStatus_Sent", resourceCulture);

	internal static Icon StartSync => (Icon)ResourceManager.GetObject("StartSync", resourceCulture);

	internal static string StpCanRunAsync => ResourceManager.GetString("StpCanRunAsync", resourceCulture);

	internal static string String => ResourceManager.GetString("String", resourceCulture);

	internal static Icon Syncing => (Icon)ResourceManager.GetObject("Syncing", resourceCulture);

	internal static Icon TimeAttendance => (Icon)ResourceManager.GetObject("TimeAttendance", resourceCulture);

	internal Resources()
	{
	}
}
