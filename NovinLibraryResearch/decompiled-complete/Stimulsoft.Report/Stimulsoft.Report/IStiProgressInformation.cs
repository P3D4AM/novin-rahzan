namespace Stimulsoft.Report;

public interface IStiProgressInformation
{
	bool IsBreaked { get; set; }

	bool IsMarquee { get; set; }

	bool AllowUseDoEvents { get; set; }

	bool IsVisible { get; }

	void Start(string title);

	void Start(string title, int progressMaximum);

	void SetProgressBar(int value, int maximum);

	void Show();

	void Hide();

	void HideProgressBar();

	void ShowProgressBar();

	void Update(string value);

	void Update(string value, int progressValue);

	void Close();

	void SetAllowClose(bool allows);
}
