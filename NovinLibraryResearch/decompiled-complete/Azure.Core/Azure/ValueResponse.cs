namespace Azure;

internal class ValueResponse<T> : Response<T>
{
	private readonly Response _response;

	public override T Value { get; }

	public ValueResponse(Response response, T value)
	{
		_response = response;
		Value = value;
	}

	public override Response GetRawResponse()
	{
		return _response;
	}
}
