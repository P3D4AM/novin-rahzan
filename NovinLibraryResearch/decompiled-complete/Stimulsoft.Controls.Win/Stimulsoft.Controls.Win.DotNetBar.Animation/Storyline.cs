using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Stimulsoft.Controls.Win.DotNetBar.Animation;

[ToolboxItem(false)]
internal class Storyline : Component
{
	private List<Animation> _Animations = new List<Animation>();

	private BackgroundWorker _Worker;

	private int _RepeatStoryTimes;

	public List<Animation> Animations => _Animations;

	public int RepeatStoryTimes
	{
		get
		{
			return _RepeatStoryTimes;
		}
		set
		{
			_RepeatStoryTimes = value;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			BackgroundWorker worker = _Worker;
			if (worker != null)
			{
				worker.CancelAsync();
				worker.Dispose();
				_Worker = null;
			}
		}
		base.Dispose(disposing);
	}

	public void Run()
	{
		if (_Worker != null)
		{
			throw new InvalidOperationException("Storyline is already running animations");
		}
		_Worker = new BackgroundWorker();
		_Worker.WorkerReportsProgress = true;
		_Worker.WorkerSupportsCancellation = true;
		_Worker.DoWork += WorkerDoWork;
		_Worker.RunWorkerCompleted += RunWorkerCompleted;
		_Worker.RunWorkerAsync();
	}

	private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (_RepeatStoryTimes > 0)
		{
			if (_Worker == null)
			{
				_RepeatStoryTimes = 0;
				return;
			}
			_RepeatStoryTimes--;
			_Worker.RunWorkerAsync();
		}
		else
		{
			_Worker.Dispose();
			_Worker = null;
		}
	}

	private void WorkerDoWork(object sender, DoWorkEventArgs e)
	{
		int count = _Animations.Count;
		for (int i = 0; i < count; i++)
		{
			if (e.Cancel)
			{
				break;
			}
			_Animations[i].Start();
			while (!_Animations[i].IsCompleted)
			{
				Thread.Sleep(50);
				if (e.Cancel)
				{
					_Animations[i].Stop();
					return;
				}
			}
		}
	}
}
