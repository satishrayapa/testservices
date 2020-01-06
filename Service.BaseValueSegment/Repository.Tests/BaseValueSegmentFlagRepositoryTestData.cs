using System;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.Constants;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Tests
{
  public static class BaseValueSegmentFlagRepositoryTestData
  {
    public static void BuildBaseValueSegmentFlags( this AumentumContext aumentumContext )
    {
      // ** Flag Not found because inactive effective status. **
      aumentumContext.FlagHeaders.Add( new FlagHeader
                                       {
                                         Id = 131,
                                         BeginEffectiveDate = new DateTime( 2011, 3, 1 ),
                                         EffectiveStatus = EffectiveStatuses.Inactive,
                                         FlagHeaderTypeId = 213 // This joins with SysType.
                                       } );

      aumentumContext.FlagRoles.Add( new FlagRole
                                     {
                                       Id = 10,
                                       BeginEffectiveDate = new DateTime( 2011, 3, 1 ),
                                       EffectiveStatus = EffectiveStatuses.Inactive,
                                       FlagHeaderId = 131,
                                       ObjectType = SystemTypes.RevenueObject,
                                       ObjectId = 13,
                                       Status = EffectiveStatuses.Active,
                                       TerminationDate = new DateTime( 9999, 12, 31 ),
                                       StartDate = new DateTime( 2001, 12, 31 )
                                     } );

      aumentumContext.SystemTypes.Add( new SystemType
                                       {
                                         Id = 213, // This references FlagHeaderTypeId  on FlagHeader.
                                         BeginEffectiveDate = new DateTime( 2011, 3, 1 ),
                                         Description = "I am missing",
                                         EffectiveStatus = EffectiveStatuses.Inactive
                                       } );

      // ** Single Base Value Segment Flag for RevObjId = 15. **
      aumentumContext.FlagHeaders.Add( new FlagHeader
                                       {
                                         Id = 132,
                                         BeginEffectiveDate = new DateTime( 2012, 3, 11 ),
                                         EffectiveStatus = EffectiveStatuses.Active,
                                         FlagHeaderTypeId = 215 // This joins with SysType.
                                       } );

      aumentumContext.FlagRoles.Add( new FlagRole
                                     {
                                       Id = 11,
                                       BeginEffectiveDate = new DateTime( 2012, 3, 11 ),
                                       EffectiveStatus = EffectiveStatuses.Active,
                                       FlagHeaderId = 132,
                                       ObjectType = SystemTypes.RevenueObject,
                                       ObjectId = 15,
                                       TerminationDate = new DateTime( 9999, 12, 31 ),
                                       Status = EffectiveStatuses.Active,
                                       StartDate = new DateTime( 2001, 12, 31 )
                                     } );

      aumentumContext.SystemTypes.Add( new SystemType
                                       {
                                         Id = 215, // This references FlagHeaderTypeId  on FlagHeader.
                                         BeginEffectiveDate = new DateTime( 2012, 3, 11 ),
                                         Description = "foobar",
                                         EffectiveStatus = EffectiveStatuses.Active
                                       } );

      // ** Two Base Value Segment Flags for RevObjId = 18. **
      aumentumContext.FlagHeaders.Add( new FlagHeader
                                       {
                                         Id = 140,
                                         BeginEffectiveDate = new DateTime( 2015, 6, 19 ),
                                         EffectiveStatus = EffectiveStatuses.Active,
                                         FlagHeaderTypeId = 228 // This joins with SysType.
                                       } );

      aumentumContext.FlagHeaders.Add( new FlagHeader
                                       {
                                         Id = 141,
                                         BeginEffectiveDate = new DateTime( 2015, 6, 19 ),
                                         EffectiveStatus = EffectiveStatuses.Active,
                                         FlagHeaderTypeId = 229 // This joins with SysType.
                                       } );

      aumentumContext.FlagRoles.Add( new FlagRole
                                     {
                                       Id = 12,
                                       BeginEffectiveDate = new DateTime( 2015, 6, 19 ),
                                       EffectiveStatus = EffectiveStatuses.Active,
                                       FlagHeaderId = 140,
                                       ObjectType = SystemTypes.RevenueObject,
                                       ObjectId = 18,
                                       TerminationDate = new DateTime( 9999, 12, 31 ),
                                       Status = EffectiveStatuses.Active,
                                       StartDate = new DateTime( 2001, 12, 31 )
                                     } );

      aumentumContext.FlagRoles.Add( new FlagRole
                                     {
                                       Id = 13,
                                       BeginEffectiveDate = new DateTime( 2015, 6, 19 ),
                                       EffectiveStatus = EffectiveStatuses.Active,
                                       FlagHeaderId = 141,
                                       ObjectType = SystemTypes.RevenueObject,
                                       ObjectId = 18,
                                       TerminationDate = new DateTime( 9999, 12, 31 ),
                                       Status = EffectiveStatuses.Active,
                                       StartDate = new DateTime( 2001, 12, 31 )
                                     } );

      aumentumContext.SystemTypes.Add( new SystemType
                                       {
                                         Id = 228, // This references FlagHeaderTypeId  on FlagHeader.
                                         BeginEffectiveDate = new DateTime( 2015, 6, 19 ),
                                         Description = "foo",
                                         EffectiveStatus = EffectiveStatuses.Active
                                       } );

      aumentumContext.SystemTypes.Add( new SystemType
                                       {
                                         Id = 229, // This references FlagHeaderTypeId  on FlagHeader.
                                         BeginEffectiveDate = new DateTime( 2015, 6, 19 ),
                                         Description = "bar",
                                         EffectiveStatus = EffectiveStatuses.Active
                                       } );

      aumentumContext.SaveChanges();
    }
  }
}
