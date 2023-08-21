namespace Search.Interfaces
{
	public interface ISearch
	{
		public const int MaxAlphabetSize = 256;

		/// <summary>                 
		/// Callback, user will received a call to specified Found delegate on matched pattern; if user code returns false, search will be aborted
		/// </summary>
		/// <param name="offset">Offset into the input buffer, zero based</param>
		/// <param name="caller">System type of the algorithm doing the search</param>
		/// <returns></returns>
		delegate bool OnMatchFoundDelegate(int offset, Type caller);

		/// <summary>
		/// Late initialization method, used to support parameterless ctors - after the instance creation, call the Init()
		/// </summary>
		/// <param name="patternMemory">Memory pattern we are searching for, for any future input buffer provided</param>
		/// <param name="patternMatched">User specified callback function, provides offset of the match in the input block and information who is the caller</param>
		abstract void Init(in ReadOnlyMemory<byte> patternMemory, OnMatchFoundDelegate patternMatched);

		/// <summary>
		/// Actual search, only a reference to the buffer and offset inside it is needed.
		/// Matches will be reported via the delegate specified in the c-tor or in the call to Init() method
		/// </summary>
		/// <param name="bufferMemory">User provided input buffer where we should search for the pattern</param>
		/// <param name="offset">Offset from which we will start or continue looking for the pattern, from the begging of the supplied input buffer (zero)</param>
		abstract void Search(in ReadOnlyMemory<byte> bufferMemory, int offset, int size);

		/// <summary>
		/// Validates internal object state.
		/// Checks whether all essential fields are properly initialized (usually that means not null).
		/// This is necessary because of the late initialization.
		/// </summary>
		abstract void Validate();

		/// <summary>
		/// For specific algorithms, returns whether modification of the search buffer is needed
		/// </summary>
		/// <returns>when true, it advises a caller to perform additional call to GetEnlargedBuffer() and then perform search on returned buffer</returns>
		abstract bool IsEnlargementNeeded();

		/// <summary>
		/// Creates a copy of the search buffer, enlarges it as needed and returns separately bufferSize (old size) and buffer
		/// </summary>
		/// <param name="buffer">original buffer</param>
		/// <param name="pattern">search pattern</param>
		/// <param name="bufferSize">original buffer length</param>
		/// <param name="enlargedBuffer">copy of the original buffer</param>
		abstract void GetEnlargedBuffer(in ReadOnlyMemory<byte> buffer, in ReadOnlyMemory<byte> pattern, out int bufferSize, out byte[] enlargedBuffer);
	};
};
