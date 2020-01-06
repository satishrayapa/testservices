namespace TAGov.Common
{
	/// <summary>
	/// Api specific exceptions.
	/// </summary>
	public class ApiExceptionMessage
	{
		public ApiExceptionMessage(string message)
		{
			Message = message;
		}

		/// <summary>
		/// Gets or sets the user-friendly message to display.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the error Id referenced in the log repository.
		/// </summary>
		public string ErrorId { get; set; }
	}
}
