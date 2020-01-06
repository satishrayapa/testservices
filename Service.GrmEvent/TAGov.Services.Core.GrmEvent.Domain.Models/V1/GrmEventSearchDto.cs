using System.Collections.Generic;

namespace TAGov.Services.Core.GrmEvent.Domain.Models.V1
{
  /// <summary>
  /// GRM Event Search specific DTO.
  /// </summary>
  public class GrmEventSearchDto
  {
    /// <summary>
    /// public constructor for GrmEventSearchDto
    /// </summary>
    public GrmEventSearchDto()
    {
      this.GrmEventIdList = new List<int>();
    }

    /// <summary>
    /// Gets or sets a list of grm event Ids that you want to retrieve.
    /// </summary>
    public List<int> GrmEventIdList { get; private set; }


  }
}