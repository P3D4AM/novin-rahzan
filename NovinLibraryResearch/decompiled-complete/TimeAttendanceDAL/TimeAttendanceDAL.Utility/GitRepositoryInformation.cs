using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace TimeAttendanceDAL.Utility;

public class GitRepositoryInformation : IDisposable
{
	[CompilerGenerated]
	private sealed class _003Cget_Log_003Ed__12 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IDisposable, IEnumerator
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public GitRepositoryInformation _003C_003E4__this;

		private int _003Cskip_003E5__2;

		string IEnumerator<string>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003Cget_Log_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			GitRepositoryInformation gitRepositoryInformation = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
			}
			else
			{
				_003C_003E1__state = -1;
				_003Cskip_003E5__2 = 0;
			}
			string value = gitRepositoryInformation.RunCommand($"log --skip={_003Cskip_003E5__2++} -n1");
			if (string.IsNullOrWhiteSpace(value))
			{
				return false;
			}
			_003C_003E2__current = value;
			_003C_003E1__state = 1;
			return true;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003Cget_Log_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Log_003Ed__12(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	private bool _disposed;

	private readonly Process _gitProcess;

	public string CommitHash => RunCommand("rev-parse HEAD");

	public string BranchName => RunCommand("rev-parse --abbrev-ref HEAD");

	public string TrackedBranchName => RunCommand("rev-parse --abbrev-ref --symbolic-full-name @{u}");

	public bool HasUnpushedCommits => !string.IsNullOrWhiteSpace(RunCommand("log @{u}..HEAD"));

	public bool HasUncommittedChanges => !string.IsNullOrWhiteSpace(RunCommand("status --porcelain"));

	public IEnumerable<string> Log
	{
		[IteratorStateMachine(typeof(_003Cget_Log_003Ed__12))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Log_003Ed__12(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	private bool IsGitRepository => !string.IsNullOrWhiteSpace(RunCommand("log -1"));

	public static GitRepositoryInformation GetRepositoryInformationForPath(string path, string gitPath = null)
	{
		GitRepositoryInformation gitRepositoryInformation = new GitRepositoryInformation(path, gitPath);
		if (gitRepositoryInformation.IsGitRepository)
		{
			return gitRepositoryInformation;
		}
		return null;
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			_disposed = true;
			_gitProcess.Dispose();
		}
	}

	public GitRepositoryInformation(string path, string gitPath)
	{
		ProcessStartInfo startInfo = new ProcessStartInfo
		{
			UseShellExecute = false,
			RedirectStandardOutput = true,
			FileName = (Directory.Exists(gitPath) ? gitPath : "git.exe"),
			CreateNoWindow = true,
			WorkingDirectory = ((path != null && Directory.Exists(path)) ? path : Environment.CurrentDirectory)
		};
		_gitProcess = new Process();
		_gitProcess.StartInfo = startInfo;
	}

	private string RunCommand(string args)
	{
		_gitProcess.StartInfo.Arguments = args;
		_gitProcess.Start();
		string result = _gitProcess.StandardOutput.ReadToEnd().Trim();
		_gitProcess.WaitForExit();
		return result;
	}
}
