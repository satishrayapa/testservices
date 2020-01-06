using System;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TAGov.Services.Core.AssessmentEvent.Repository;
using TAGov.Services.Core.AssessmentEvent.Repository.Implementation.V1;
using TAGov.Services.Core.AssessmentEvent.Repository.Interfaces.V1;
using TAGov.Services.Core.AssessmentEvent.Repository.Models.V1;
using Xunit;
using ValueType = TAGov.Services.Core.AssessmentEvent.Repository.Models.V1.ValueType;

namespace Repository.Tests
{

	public class RepositoryTests
	{
		private const int AssessmentEventId = 999999999;
		private const int AssessmentEventTransactionId = 888888888;
		private const int AssessmentEventTypeId = 777777777;
		private const string AssessmentEventTypeDescription = "Transfer";

		private const int AssessmentEventTypeIdOlder = 666666666;
		private const string AssessmentEventTypeDescriptionOlder = "Too old";

		private const int AssessmentEventStateId = 555555555;
		private const string AssessmentEventStateDescription = "Review Required";
		private const int AssessmentEventStateIdOlder = 44444444;
		private const string AssessmentEventStateDescriptionOlder = "Too old event state";

		private const int AssessmentRevisionEventId = 3;
		private const int AssessmentRevisionId = 4;
		private const string AssessmentRevisionReferenceNumber = "None";
		private readonly DateTime _assessmentRevisionDate = new DateTime(1999, 1, 1);

		private const string ChangeReason = "Some change reason";
		private const int ChangeReasonId = 5;

		private const int RevenueObjectId = 5;

		private const int SysTypeIdAsmtRevn = 6;
		private const int SysTypeCatIdObjectType = 7;
		private const int SysTypePublicUse = 8;
		private const int SysTypeIdPrimaryBaseYearAttribute2 = 9;
		private const string SystemTypeLongDescription = "foobar";
		private const string NoteText = "some note text";
		private const string Attribute2DescriptionMultiple = "M";

		private readonly AssessmentEventContext _context;

		private static readonly AssessmentEventValue AssessmentEventValueBvs
			= new AssessmentEventValue
			  {
				  Id = 3333,
				  AsmtEventTranId = AssessmentEventTransactionId,
				  DynCalcStepTrackingId = 0,
				  ValueTypeId = 1000051,
				  TaxYear = 1999,
				  Attribute1 = 1111,
				  Attribute2 = 2222,
				  ValueAmount = 5555
			  };

		private static readonly AssessmentEventValue AssessmentEventValueBaseYear4Asmt
			= new AssessmentEventValue
			  {
				  Id = 1,
				  AsmtEventTranId = AssessmentEventTransactionId,
				  DynCalcStepTrackingId = 2,
				  ValueTypeId = 1000316,
				  TaxYear = 2001,
				  Attribute1 = 3,
				  Attribute2 = SysTypeIdPrimaryBaseYearAttribute2,
				  Attribute2Description = Attribute2DescriptionMultiple,
				  ValueAmount = 7
			  };

	public RepositoryTests()
		{
			var optionsBuilder = new DbContextOptionsBuilder<AssessmentEventContext>();
			optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString("N"));

			_context = new AssessmentEventContext(optionsBuilder);

			_context.AssessmentEvent.Add(new AssessmentEvent
			{
				Id = AssessmentEventId,
				TranId = 0,
				DynCalcStepTrackingId = 0,
				RevObjId = RevenueObjectId,
				TaxYear = 0,
				AsmtEventType = AssessmentEventTypeId,
				EventDate = DateTime.Now
			});


			_context.AssessmentEventTransaction.Add(new AssessmentEventTransaction
			{
				Id = AssessmentEventTransactionId,
				AsmtEventId = AssessmentEventId,
				AsmtEventState = AssessmentEventStateId,
				AsmtRevnEventId = 0
			});


			_context.AssessmentEventTransaction.Add(new AssessmentEventTransaction
			{
				Id = AssessmentEventTransactionId + 1,
				AsmtEventId = AssessmentEventId,
				AsmtEventState = AssessmentEventStateId,
				AsmtRevnEventId = 0
			});

			_context.AssessmentEventValue.Add( new AssessmentEventValue
			                                   {
				                                   AsmtEventTranId = AssessmentEventValueBvs.AsmtEventTranId,
				                                   Attribute1 = AssessmentEventValueBvs.Attribute1,
				                                   Attribute2 = AssessmentEventValueBvs.Attribute2,
				                                   DynCalcStepTrackingId = AssessmentEventValueBvs.DynCalcStepTrackingId,
				                                   Id = AssessmentEventValueBvs.Id,
				                                   TaxYear = AssessmentEventValueBvs.TaxYear,
				                                   ValueAmount = AssessmentEventValueBvs.ValueAmount,
				                                   ValueTypeId = AssessmentEventValueBvs.ValueTypeId
			                                   } );

			_context.AssessmentEventValue.Add( new AssessmentEventValue
			                                   {
				                                   AsmtEventTranId = AssessmentEventValueBaseYear4Asmt.AsmtEventTranId,
				                                   Attribute1 = AssessmentEventValueBaseYear4Asmt.Attribute1,
				                                   Attribute2 = AssessmentEventValueBaseYear4Asmt.Attribute2,
				                                   DynCalcStepTrackingId = AssessmentEventValueBaseYear4Asmt.DynCalcStepTrackingId,
				                                   Id = AssessmentEventValueBaseYear4Asmt.Id,
				                                   TaxYear = AssessmentEventValueBaseYear4Asmt.TaxYear,
				                                   ValueAmount = AssessmentEventValueBaseYear4Asmt.ValueAmount,
				                                   ValueTypeId = AssessmentEventValueBaseYear4Asmt.ValueTypeId
			                                   } );

			_context.ValueType.Add(new ValueType
			{
				Id = 1000051,
				ShortDescr = "BVS"
			});

			_context.ValueType.Add(new ValueType
			{
				Id = 1000052,
				ShortDescr = "BVS2"
			});

			_context.ValueType.Add(new ValueType
			{
				Id = 1000053,
				ShortDescr = "BVSValue"
			});

			_context.ValueType.Add(new ValueType
			{
				Id = 1000057,
				ShortDescr = "BVSTrend"
			});

			_context.ValueType.Add(new ValueType
			{
				Id = 1000083,
				ShortDescr = "BVSCalamity"
			});

			_context.ValueType.Add(new ValueType
			{
				Id = 1000097,
				ShortDescr = "BVSTransfer"
			});

			_context.ValueType.Add( new ValueType
			                        {
				                        Id = 1000316,
				                        ShortDescr = "BaseYear4Asmt"
			                        } );

			_context.SysTypeCat.Add(new SysTypeCat
			{
				Id = SysTypeCatIdObjectType,
				ShortDescription = "Object Type",
				BeginEffectiveDate = new DateTime(1776, 1, 1),
				EffectiveStatus = "A"
			});

			_context.SystemType.Add(new SystemType
			{
				Id = 470404,
				begEffDate = new DateTime(1776, 07, 04),
				shortDescr = "None",
				descr = SystemTypeLongDescription
			});
			_context.SystemType.Add(new SystemType
			{
				Id = 470422,
				begEffDate = new DateTime(1776, 07, 04),
				shortDescr = "Other",
				descr = SystemTypeLongDescription
			});
			_context.SystemType.Add(new SystemType
			{
				Id = 470426,
				begEffDate = new DateTime(1776, 07, 04),
				shortDescr = "Year",
				descr = SystemTypeLongDescription
			});
			_context.SystemType.Add(new SystemType
			{
				Id = 470434,
				begEffDate = new DateTime(1776, 07, 04),
				shortDescr = "LegalPartyRole",
				descr = SystemTypeLongDescription
			});
			_context.SystemType.Add(new SystemType
			{
				Id = 470436,
				begEffDate = new DateTime(1776, 07, 04),
				shortDescr = "AsmtEvent",
				descr = SystemTypeLongDescription
			});
			_context.SystemType.Add(new SystemType
			{
				Id = 470475,
				begEffDate = new DateTime(1776, 07, 04),
				shortDescr = "RPAObject",
				descr = SystemTypeLongDescription
			});
			_context.SystemType.Add(new SystemType
			{
				Id = AssessmentEventTypeId,
				begEffDate = new DateTime(1776, 07, 04),
				shortDescr = AssessmentEventTypeDescription,
				descr = SystemTypeLongDescription
			});
			_context.SystemType.Add(new SystemType
			{
				Id = AssessmentEventTypeIdOlder,
				begEffDate = new DateTime(1776, 07, 03),
				shortDescr = AssessmentEventTypeDescriptionOlder,
				descr = SystemTypeLongDescription
			});
			_context.SystemType.Add(new SystemType
			{
				Id = AssessmentEventStateId,
				begEffDate = new DateTime(1776, 07, 04),
				shortDescr = AssessmentEventStateDescription,
				descr = SystemTypeLongDescription
			});
			_context.SystemType.Add(new SystemType
			{
				Id = AssessmentEventStateIdOlder,
				begEffDate = new DateTime(1776, 07, 03),
				shortDescr = AssessmentEventStateDescriptionOlder,
				descr = SystemTypeLongDescription
			});
			_context.SystemType.Add(new SystemType
			{
				Id = ChangeReasonId,
				begEffDate = new DateTime(1776, 07, 04),
				descr = ChangeReason
			});
			_context.SystemType.Add(new SystemType
			{
				Id = SysTypeIdAsmtRevn,
				shortDescr = "AsmtRevn",
				begEffDate = new DateTime(1776, 01, 01),
				effStatus = "A",
				sysTypeCatId = SysTypeCatIdObjectType
			});
			_context.SystemType.Add(new SystemType
			{
				Id = SysTypePublicUse,
				shortDescr = "Public use",
				begEffDate = new DateTime(1776, 1, 1),
				effStatus = "A"
			});
			_context.SystemType.Add( new SystemType
			                         {
				                         Id = SysTypeIdPrimaryBaseYearAttribute2,
				                         descr = Attribute2DescriptionMultiple,
				                         begEffDate = new DateTime( 1776, 1, 1 ),
				                         effStatus = "A"
			                         } );

			_context.AssessmentRevision.Add(new AssessmentRevision
			{
				Id = AssessmentRevisionId,
				ReferenceNumber = AssessmentRevisionReferenceNumber,
				ValChangeReason = ChangeReasonId,
				RevisionDate = _assessmentRevisionDate
			});

			_context.AssessmentRevisionEvent.Add(new AssessmentRevisionEvent
			{
				Id = AssessmentRevisionEventId,
				AsmtRevnId = AssessmentRevisionId
			});

			_context.Note.Add(new Note
			{
				Id = 23223,
				NoteText = NoteText,
				BeginEffectiveDate = new DateTime(1776, 01, 01),
				EffectiveStatus = "A",
				ObjectType = SysTypeIdAsmtRevn,
				ObjectId = AssessmentRevisionId,
				Privacy = SysTypePublicUse
			});

			_context.SaveChanges();
		}

		[Fact]
		public void Get_InsertsAndRetrievesTheRecord_MatchesId()
		{
			IAssessmentEventRepository assesmentEventRepository = new AssessmentEventRepository(_context, null);
			var assessmentEvent = assesmentEventRepository.GetAsync(AssessmentEventId).Result;

			assessmentEvent.ShouldNotBeNull();
			assessmentEvent.Id.ShouldBe(AssessmentEventId);
			assessmentEvent.AsmtEventTypeDescription.ShouldBe(SystemTypeLongDescription);
			assessmentEvent.RevObjId.ShouldBe(RevenueObjectId);
		}

		[Fact]
		public void Get_InsertsAndRetrievesTheRecord_DoesNotMatchId()
		{
			IAssessmentEventRepository assesmentEventRepository = new AssessmentEventRepository(_context, null);
			var assesmentEvent = assesmentEventRepository.GetAsync(0).Result;

			assesmentEvent.ShouldBeNull();
		}

		[Fact]
		public void GetByAssessmentEventTransactionId_InsertsAndRetrievesTheRecord_DoesNotMatchId()
		{
			IAssessmentEventRepository assesmentEventRepository = new AssessmentEventRepository(_context, null);
			var assesmentEvents = assesmentEventRepository.GetAsync(0).Result;

			assesmentEvents.ShouldBeNull();
		}


		[Fact]
		public void GetAssessmentRevisionByAssessmentRevisionEventId_InsertsAndRetrievesTheRecord_MatchesId()
		{
			IAssessmentEventRepository assessmentEventRepository = new AssessmentEventRepository(_context, null);
			var assessmentRevision =
				assessmentEventRepository.GetAssessmentRevisionByAssessmentRevisionEventId(AssessmentRevisionEventId, new DateTime(2016, 1, 1));

			assessmentRevision.ShouldNotBeNull();
			assessmentRevision.Id.ShouldBe(AssessmentRevisionId);
			assessmentRevision.ReferenceNumber.ShouldBe(AssessmentRevisionReferenceNumber);
			assessmentRevision.ChangeReason.ShouldBe(ChangeReason);
			assessmentRevision.Note.ShouldBe(NoteText);
		}

		[Fact]
		public void GetAssessmentRevisionByAssessmentRevisionEventId_InsertsAndRetrievesTheRecord_DoesNotMatchId()
		{
			IAssessmentEventRepository assessmentEventRepository = new AssessmentEventRepository(_context, null);
			var assessmentRevision = assessmentEventRepository.GetAssessmentRevisionByAssessmentRevisionEventId(AssessmentRevisionId, new DateTime(2016, 1, 1));  //not the id

			assessmentRevision.ShouldBeNull();
		}

		[Fact]
		public void GetAssessmentEventValueByAssessmentEventTransactionId_RetrievesRecords_MatchesId()
		{
			IAssessmentEventRepository assessmentEventRepository = new AssessmentEventRepository( _context, null );
			AssessmentEventValue assessmentEventValue = assessmentEventRepository.GetAssessmentEventValueByAssessmentEventTransactionIdAsync( AssessmentEventTransactionId ).Result;
			assessmentEventValue.Attribute1.ShouldBe( AssessmentEventValueBaseYear4Asmt.Attribute1 );
			assessmentEventValue.Attribute2.ShouldBe( AssessmentEventValueBaseYear4Asmt.Attribute2 );
			assessmentEventValue.DynCalcStepTrackingId.ShouldBe( AssessmentEventValueBaseYear4Asmt.DynCalcStepTrackingId );
			assessmentEventValue.Id.ShouldBe( AssessmentEventValueBaseYear4Asmt.Id );
			assessmentEventValue.TaxYear.ShouldBe( AssessmentEventValueBaseYear4Asmt.TaxYear );
			assessmentEventValue.ValueAmount.ShouldBe( AssessmentEventValueBaseYear4Asmt.ValueAmount );
			assessmentEventValue.ValueTypeId.ShouldBe( AssessmentEventValueBaseYear4Asmt.ValueTypeId );
			assessmentEventValue.AsmtEventTranId.ShouldBe( AssessmentEventValueBaseYear4Asmt.AsmtEventTranId );
			assessmentEventValue.Attribute2Description.ShouldBe(AssessmentEventValueBaseYear4Asmt.Attribute2Description);
		}

		[Fact]
		public void GetAssessmentEventValueByAssessmentEventTransactionId_RetrievesRecord_DoesNotMatchId()
		{
			IAssessmentEventRepository assessmentEventRepository = new AssessmentEventRepository( _context, null );
			AssessmentEventValue assessmentEventValue = assessmentEventRepository.GetAssessmentEventValueByAssessmentEventTransactionIdAsync( AssessmentEventTransactionId + 1 ).Result;
			assessmentEventValue.ShouldBeNull();
		}
	}
}
