using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration;

public abstract class FileConfigurationProvider : ConfigurationProvider, IDisposable
{
	private readonly IDisposable _changeTokenRegistration;

	public FileConfigurationSource Source { get; }

	public FileConfigurationProvider(FileConfigurationSource source)
	{
		Source = source ?? throw new ArgumentNullException("source");
		if (Source.ReloadOnChange && Source.FileProvider != null)
		{
			_changeTokenRegistration = ChangeToken.OnChange(() => Source.FileProvider.Watch(Source.Path), delegate
			{
				Thread.Sleep(Source.ReloadDelay);
				Load(reload: true);
			});
		}
	}

	public override string ToString()
	{
		return GetType().Name + " for '" + Source.Path + "' (" + (Source.Optional ? "Optional" : "Required") + ")";
	}

	private void Load(bool reload)
	{
		IFileInfo fileInfo2 = Source.FileProvider?.GetFileInfo(Source.Path);
		if (fileInfo2 == null || !fileInfo2.Exists)
		{
			if (Source.Optional || reload)
			{
				base.Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder(System.SR.Format(System.SR.Error_FileNotFound, Source.Path));
				if (!string.IsNullOrEmpty(fileInfo2?.PhysicalPath))
				{
					stringBuilder.Append(System.SR.Format(System.SR.Error_ExpectedPhysicalPath, fileInfo2.PhysicalPath));
				}
				HandleException(ExceptionDispatchInfo.Capture(new FileNotFoundException(stringBuilder.ToString())));
			}
		}
		else
		{
			using Stream stream = OpenRead(fileInfo2);
			try
			{
				Load(stream);
			}
			catch (Exception innerException)
			{
				if (reload)
				{
					base.Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				}
				InvalidDataException source = new InvalidDataException(System.SR.Format(System.SR.Error_FailedToLoad, fileInfo2.PhysicalPath), innerException);
				HandleException(ExceptionDispatchInfo.Capture(source));
			}
		}
		OnReload();
		static Stream OpenRead(IFileInfo fileInfo)
		{
			if (fileInfo.PhysicalPath != null)
			{
				return new FileStream(fileInfo.PhysicalPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1, FileOptions.SequentialScan);
			}
			return fileInfo.CreateReadStream();
		}
	}

	public override void Load()
	{
		Load(reload: false);
	}

	public abstract void Load(Stream stream);

	private void HandleException(ExceptionDispatchInfo info)
	{
		bool flag = false;
		if (Source.OnLoadException != null)
		{
			FileLoadExceptionContext fileLoadExceptionContext = new FileLoadExceptionContext
			{
				Provider = this,
				Exception = info.SourceException
			};
			Source.OnLoadException(fileLoadExceptionContext);
			flag = fileLoadExceptionContext.Ignore;
		}
		if (!flag)
		{
			info.Throw();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
		_changeTokenRegistration?.Dispose();
	}
}
