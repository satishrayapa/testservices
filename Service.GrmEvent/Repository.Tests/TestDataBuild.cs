using System;
using TAGov.Services.Core.GrmEvent.Repository.Models.V1;

namespace TAGov.Services.Core.GrmEvent.Repository.Tests
{
  public static class TestDataBuilder
  {
    public const int GrmEventId1 = 309431;
    public const int GrmEventRoleId1 = 284518;
    public const int GrmEventRoleId2 = 284519;

    public const int GrmEventId2 = 309432;
    public const int GrmEventRoleId3 = 284519;

    public const int GrmEventId = 44;
    public const int SystemTypeId = 55;
    public const int RevObjId = 66;


    public static void Build( GrmEventContext grmEventContext )
    {


      grmEventContext.GrmEvent.Add( new Models.V1.GrmEvent
                                    {
                                      Id = GrmEventId,
                                      EffDate = new DateTime( 2011, 01, 01 ),
                                      EventType = SystemTypeId
                                    } );

      grmEventContext.GrmEvent.Add( new Models.V1.GrmEvent
                                    {
                                      Id = GrmEventId + 1,
                                      EffDate = new DateTime( 2011, 01, 01 ),
                                      EventType = SystemTypeId
                                    } );

      grmEventContext.GrmEvent.Add( new Models.V1.GrmEvent
                                    {
                                      Id = GrmEventId + 2,
                                      EffDate = new DateTime( 2011, 01, 01 ),
                                      EventType = SystemTypeId
                                    } );

      grmEventContext.SystemType.Add( new SystemType
                                      {
                                        Id = SystemTypeId,
                                        begEffDate = new DateTime( 2011, 01, 01 ),
                                        effStatus = "A",
                                        shortDescr = "ACTIVE",
                                        descr = "ACTIVE",
                                        displayOrder = 0,
                                        sysTypeCatId = 1,
                                        tranId = 1000,
                                        editShortDescr = 1,
                                        editDescr = 1,
                                        canDeleteRow = 0,
                                        canSelectRow = 1,
                                        InternalId = 10055,
                                        depSysTypeId = 0
                                      } );

      grmEventContext.SystemType.Add( new SystemType
                                      {
                                        Id = SystemTypeId,
                                        begEffDate = new DateTime( 2011, 01, 02 ),
                                        effStatus = "I",
                                        shortDescr = "INACTIVE",
                                        descr = "INACTIVE"

                                      } );

      grmEventContext.GrmEventInformation.Add( new GrmEventInformation
                                               {
                                                 GrmEventId = GrmEventId,
                                                 Description = "UnitTestGrmEventInfo",
                                                 EffectiveDate = new DateTime( 2011, 01, 01 ),
                                                 EventType = "Transfer",
                                                 EventDate = new DateTime( 2011, 02, 01 ),
                                                 RevenueObjectId = 100
                                               } );

      grmEventContext.GrmEvent.Add( new Models.V1.GrmEvent
                                    {
                                      Id = GrmEventId + 3,
                                      TranId = -1000,
                                      EventType = SystemTypeId,
                                      RevObjId = RevObjId,
                                      EffDate = new DateTime( 2016, 1, 1 )
                                    } );

      grmEventContext.SaveChanges();
    }
  }
}