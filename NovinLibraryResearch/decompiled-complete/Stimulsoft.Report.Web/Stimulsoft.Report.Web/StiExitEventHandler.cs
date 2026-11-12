using System;
using System.ComponentModel;

namespace Stimulsoft.Report.Web;

[Obsolete("This delegate is obsolete. It will be removed in next versions. Please use the StiReportDataEventHandler delegate instead.")]
[EditorBrowsable(EditorBrowsableState.Never)]
public delegate void StiExitEventHandler(object sender, StiExitEventArgs e);
