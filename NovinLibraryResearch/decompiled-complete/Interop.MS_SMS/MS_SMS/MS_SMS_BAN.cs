using System.Runtime.InteropServices;

namespace MS_SMS;

[ComImport]
[Guid("9686C09B-763D-474F-AF5D-140EB90C59D0")]
[CoClass(typeof(MS_SMS_BANClass))]
public interface MS_SMS_BAN : IMS_SMS_BAN, IMS_SMS_BANEvents_Event
{
}
