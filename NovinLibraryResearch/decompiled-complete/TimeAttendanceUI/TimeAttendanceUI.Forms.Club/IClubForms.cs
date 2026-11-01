namespace TimeAttendanceUI.Forms.Club;

public interface IClubForms
{
	bool TopLevel { get; set; }

	bool AutoScroll { get; set; }

	void FillFrmFields();

	IClubForms NextPage();

	void Show();
}
