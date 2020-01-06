using System.Collections.Generic;
using System.Linq;
using TAGov.Common.Exceptions;

namespace TAGov.Common
{
	public class IdInfo
	{
		public IdInfo(string key, object value)
		{
			Key = key;
			Value = value;
		}

		public string Key { get; set; }
		public object Value { get; set; }
	}
	public static class AssertExtensions
	{
		/// <summary>
		/// Throws a BadRequestException if Id is a negative number.
		/// </summary>
		/// <param name="id">Id to assert.</param>
		/// <param name="identifier">Name of the value.</param>
		public static void ThrowBadRequestExceptionIfInvalid(this int id, string identifier)
		{
			if (id < 0)
				throw new BadRequestException($"{identifier} {id} is invalid.");
		}

		/// <summary>
		/// Throws a BadRequestException if Id is null or is a negative number.
		/// </summary>
		/// <param name="id">Id to assert.</param>
		/// <param name="identifier">Name of the value.</param>
		public static void ThrowBadRequestExceptionIfInvalidOnNullable(this int? id, string identifier)
		{
			if (!id.HasValue)
				throw new BadRequestException($"{identifier} is invalid.");

			id.Value.ThrowBadRequestExceptionIfInvalid(identifier);
		}



		/// <summary>
		/// Validates that any reference type is not null and throws a BadRequestException if the object is null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">The object to validate.</param>
		/// <param name="identifier">The name or description of the object (to be included in logs).</param>
		public static void ThrowBadRequestExceptionIfNull<T>(this T item, string identifier) where T:class
		{
			if (item == null)
				throw new BadRequestException($"{identifier} is invalid."); 
		}


		/// <summary>
		/// Validates a collection by ensuring that it is not null or empty and that is contains no null members.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection">The collection to validate.</param>
		/// <param name="identifier">The name or description of the collection (to be included in logs).</param>
		public static void ThrowBadRequestExceptionIfNullOrEmpty<T>(this ICollection<T> collection, string identifier)
		{
			if (collection == null)
				throw new BadRequestException($"{identifier} is invalid.");

			if (collection.Count == 0)
				throw new BadRequestException($"{identifier} collection is empty.");

			foreach (T member in collection)
			{
				if (member == null)
					throw new BadRequestException($"{identifier} collection contains null members.");
			}
		}


	

		public static void ThrowRecordNotFoundExceptionIfNull<T>(this T item, params IdInfo[] idInfos)
		{
			if (item == null)
			{
				var typeOfT = typeof(T);
				var recordId = string.Join(",", idInfos.Select(x => $"{x.Key}={x.Value}"));
				string identifierName = typeOfT.Name;
				throw new RecordNotFoundException(recordId, typeOfT, $"No record can be found for {identifierName} with the following key(s): {recordId}.");
			}
		}
	}
}
