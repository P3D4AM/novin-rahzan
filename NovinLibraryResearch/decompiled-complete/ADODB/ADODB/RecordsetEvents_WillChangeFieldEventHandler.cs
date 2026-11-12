using System.Runtime.InteropServices;

namespace ADODB;

[ComVisible(false)]
public delegate void RecordsetEvents_WillChangeFieldEventHandler([In] int cFields, [In][MarshalAs(UnmanagedType.Struct)] object Fields, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset);
