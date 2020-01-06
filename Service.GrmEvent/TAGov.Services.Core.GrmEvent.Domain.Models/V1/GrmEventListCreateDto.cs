using System.Collections.Generic;

namespace TAGov.Services.Core.GrmEvent.Domain.Models.V1
{
  public class GrmEventListCreateDto
  {
    public GrmEventListCreateDto()
    {
      GrmEventList = new List<GrmEventCreateDto>();
    }

    public List<GrmEventCreateDto> GrmEventList { get; set; }
  }
}