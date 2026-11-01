using System;
using System.Drawing;
using TimeAttendanceDAL.Properties;

namespace TimeAttendanceDAL.Dal.Device;

public class Person_Hardwarecodes
{
	public HardwareCodeCUSView HardwareCodeCusView { get; set; }

	public int RowNumber { get; set; } = 1;


	public int DeviceId => HardwareCodeCusView.DeviceId;

	public Guid DeviceGuid => HardwareCodeCusView.DeviceGuid;

	public int PersonId => HardwareCodeCusView.PersonId;

	public Guid PersonGuid => HardwareCodeCusView.PersonGuid;

	public string DeviceName => HardwareCodeCusView.DeviceName;

	public string HardWareCode { get; set; }

	public string PersonFullName => HardwareCodeCusView.FullName;

	public string NameInDevice { get; set; }

	public bool IsSelected { get; set; }

	public Image InsertInTableImage { get; set; } = (Image)(object)Resources.question_mark;


	public Image InsertInDevice { get; set; } = (Image)(object)Resources.question_mark;

}
