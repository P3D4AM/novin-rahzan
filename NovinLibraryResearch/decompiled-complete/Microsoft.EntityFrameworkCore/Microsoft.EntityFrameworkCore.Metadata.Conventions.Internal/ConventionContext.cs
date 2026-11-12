namespace Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

public class ConventionContext<TMetadata> : IConventionContext<TMetadata>, IConventionContext, IReadableConventionContext
{
	private bool _stopProcessing;

	private readonly ConventionDispatcher _dispatcher;

	private TMetadata _result;

	public virtual TMetadata Result => _result;

	public ConventionContext(ConventionDispatcher dispatcher)
	{
		_dispatcher = dispatcher;
	}

	public virtual void StopProcessing()
	{
		_stopProcessing = true;
		_result = default(TMetadata);
	}

	public virtual void StopProcessing(TMetadata result)
	{
		_stopProcessing = true;
		_result = result;
	}

	public virtual void StopProcessingIfChanged(TMetadata result)
	{
		if (!object.Equals(Result, result))
		{
			StopProcessing(result);
		}
	}

	public virtual IConventionBatch DelayConventions()
	{
		return _dispatcher.DelayConventions();
	}

	public virtual bool ShouldStopProcessing()
	{
		return _stopProcessing;
	}

	public virtual void ResetState(TMetadata input)
	{
		_stopProcessing = false;
		_result = input;
	}
}
