using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Viewer.Elements;
using Stimulsoft.Dashboard.Viewer.Elements.Table;

namespace Stimulsoft.Dashboard.Viewer.FullScreen;

public class StiViewerFullScreenHelper : StiFullScreenHelper
{
	internal DockStyle StoredDockStyle { get; set; }

	internal Rectangle StoredLocation { get; set; }

	internal Control StoredParent { get; set; }

	internal void Show()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		if (base.IsActivated)
		{
			return;
		}
		Form owner = ((Control)base.Viewer).FindForm();
		StoredParent = ((Control)base.Viewer).Parent;
		StoredDockStyle = ((Control)base.Viewer).Dock;
		StoredLocation = ((Control)base.Viewer).ClientRectangle;
		((Control)base.Viewer).Dock = (DockStyle)5;
		List<KeyValuePair<StiTableElementControl, bool>> list = (from c in StiDashboardViewerHelper.GetElementControlList(base.Viewer)
			where c is StiTableElementControl
			select c).Cast<StiTableElementControl>().ToDictionary((StiTableElementControl t) => t, (StiTableElementControl t) => t.IsAllColumnsInvisible).ToList();
		Form val = new Form
		{
			KeyPreview = true,
			FormBorderStyle = (FormBorderStyle)0,
			WindowState = (FormWindowState)2,
			ShowInTaskbar = false,
			Owner = owner
		};
		((Control)base.Viewer).Parent.Controls.Remove((Control)(object)base.Viewer);
		((Control)val).Controls.Add((Control)(object)base.Viewer);
		((Control)val).KeyDown += new KeyEventHandler(base.Viewer.FullScreenForm_KeyDown);
		((Control)val).Show();
		val.Activate();
		base.IsActivated = true;
		base.Viewer.RefreshViewerButtons(base.Viewer.Report);
		list.ForEach(delegate(KeyValuePair<StiTableElementControl, bool> pair)
		{
			if (pair.Value)
			{
				pair.Key.HideAllColumns();
			}
		});
	}

	internal void Close()
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		if (base.IsActivated && StoredParent != null)
		{
			base.IsActivated = false;
			Control parent = ((Control)base.Viewer).Parent;
			Form val = (Form)(object)((parent is Form) ? parent : null);
			((Control)base.Viewer).Parent.Controls.Remove((Control)(object)base.Viewer);
			StoredParent.Controls.Add((Control)(object)base.Viewer);
			((Control)base.Viewer).Dock = StoredDockStyle;
			Rectangle storedLocation = StoredLocation;
			((Control)base.Viewer).SetBounds(storedLocation.X, storedLocation.Y, storedLocation.Width, storedLocation.Height);
			val.Close();
			((Control)val).KeyDown -= new KeyEventHandler(base.Viewer.FullScreenForm_KeyDown);
			((Component)(object)val).Dispose();
			base.Viewer.RefreshViewerButtons(base.Viewer.Report);
			((Control)base.Viewer).BringToFront();
		}
	}

	public StiViewerFullScreenHelper(StiDashboardViewerControl viewer)
		: base(viewer)
	{
	}
}
