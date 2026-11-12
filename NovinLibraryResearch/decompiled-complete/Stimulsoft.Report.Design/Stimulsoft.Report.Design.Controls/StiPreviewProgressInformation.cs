using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Design.Controls;

public class StiPreviewProgressInformation : IStiProgressInformation
{
	private ProgressBarItem progress;

	public bool IsBreaked { get; set; }

	public bool IsMarquee { get; set; }

	public bool AllowUseDoEvents { get; set; }

	public bool IsVisible
	{
		get
		{
			if (progress != null)
			{
				return progress.Visible;
			}
			return false;
		}
	}

	public bool Allow => progress != null;

	public void Close()
	{
		if (Allow)
		{
			Hide();
		}
	}

	public void Hide()
	{
		if (Allow)
		{
			HideProgressBar();
		}
	}

	public void HideProgressBar()
	{
		progress.Visible = false;
	}

	public void SetAllowClose(bool allows)
	{
	}

	public void SetProgressBar(int value, int maximum)
	{
		if (progress.Value <= value)
		{
			progress.Value = value;
			progress.Maximum = maximum;
			Show();
		}
	}

	public void Show()
	{
		if (Allow)
		{
			ShowProgressBar();
		}
	}

	public void ShowProgressBar()
	{
		progress.Visible = true;
		if (progress.ContainerControl is Bar bar)
		{
			((Control)bar).Invalidate();
		}
	}

	public void Start(string title)
	{
		Start(title, -1);
	}

	public void Start(string title, int progressMaximum)
	{
		progress.Text = title;
		if (progressMaximum != -1)
		{
			progress.Maximum = progressMaximum;
		}
		Show();
	}

	public void Update(string value)
	{
		Update(value, -1);
	}

	public void Update(string value, int progressValue)
	{
		progress.Text = value;
		if (progressValue != -1)
		{
			progress.Value = progressValue;
		}
	}

	public StiPreviewProgressInformation(ProgressBarItem progress)
	{
		this.progress = progress;
	}
}
