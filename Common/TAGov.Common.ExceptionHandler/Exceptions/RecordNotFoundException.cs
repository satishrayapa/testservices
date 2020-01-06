using System;

namespace TAGov.Common.Exceptions
{
	public class RecordNotFoundException : Exception
	{
		public RecordNotFoundException(string recordId, Type type, string message) : base(message)
		{
			RecordId = recordId;
			Type = type;
		}

		public string RecordId { get; private set; }

		public Type Type { get; private set; }
	}
}
