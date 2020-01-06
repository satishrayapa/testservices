using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAGov.Common.ResourceLocator.Domain.Interfaces;
using TAGov.Common.ResourceLocator.Domain.Models.V1;
using TAGov.Common.Security.Http.Authorization;

namespace TAGov.Common.ResourceLocator.API.Controllers.V1_1
{
	/// <summary>
	/// Resource API.
	/// </summary>
	[Authorize(Constants.HasResourceVerb)]
	[ApiController]
	[ApiVersion("1.1")]
	[Route("v{version:apiVersion}/Resources")]
	public class ResourceController : ControllerBase
	{
		private readonly IResourceDomain _resourceDomain;

		/// <summary>
		/// Constructor to Resource API.
		/// </summary>
		/// <param name="resourceDomain"></param>
		public ResourceController(IResourceDomain resourceDomain)
		{
			_resourceDomain = resourceDomain;
		}


		/// <summary>
		/// Gets Resource DTO based on partition.
		/// </summary>
		/// <param name="partition"></param>
		/// <returns></returns>
		[HttpGet, Route("")]
		[ProducesResponseType(typeof(IEnumerable<ResourceDto>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
		public IActionResult List([FromQuery]string partition)
		{
			return new ObjectResult(_resourceDomain.List(partition));
		}

		/// <summary>
		/// Gets a resource.
		/// </summary>
		/// <param name="partition">partition.</param>
		/// <param name="key">key.</param>
		/// <returns></returns>
		[HttpGet, Route("{partition}/{key}")]
		[ProducesResponseType(typeof(ResourceDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
		public IActionResult Get(string partition, string key)
		{
			return new ObjectResult(_resourceDomain.Get(partition, key));
		}

		/// <summary>
		/// Updates a resource.
		/// </summary>
		/// <param name="partition">partition.</param>
		/// <param name="key">key.</param>
		/// <param name="resourceDto">resourceDto.</param>
		/// <returns></returns>
		[HttpPut, Route("{partition}/{key}")]
		[ProducesResponseType(typeof(ResourceDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
		public IActionResult Update(string partition, string key, [FromBody] ResourceDto resourceDto)
		{
			// Partition and key in DTO and query string must match as a matter of consistency.
			if (partition != resourceDto.Partition || key != resourceDto.Key)
			{
				return new BadRequestResult();
			}

			_resourceDomain.Update(resourceDto);
			return new OkObjectResult(resourceDto);
		}

		/// <summary>
		/// Creates a resource.
		/// </summary>
		/// <param name="resourceDto">resourceDto.</param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(ResourceDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
		public IActionResult Create([FromBody] ResourceDto resourceDto)
		{
			_resourceDomain.Create(resourceDto);
			return new RedirectResult($"Resources/{resourceDto.Partition}/{resourceDto.Key}");
		}

		/// <summary>
		/// Updates all resources with same partition.
		/// </summary>
		/// <param name="partition">partition.</param>
		/// <param name="resourceDtos"></param>
		/// <returns></returns>
		[HttpPut, Route("")]
		[ProducesResponseType(typeof(ResourceDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(ApiExceptionMessage), (int)HttpStatusCode.NotFound)]
		public IActionResult Update([FromQuery]string partition, [FromBody] List<ResourceDto> resourceDtos)
		{
			_resourceDomain.UpdateList(resourceDtos);
			return new OkObjectResult(resourceDtos);
		}
	}
}
