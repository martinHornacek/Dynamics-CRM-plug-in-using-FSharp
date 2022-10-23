namespace OpportunityManagement.Tests

open Microsoft.Xrm.Sdk
open OpportunityManagement
open OpportunityManagement.RatingLogic
open Xunit

module LogicTests =

    [<Fact>]
    let ``RatingLogic.getEntityType returns Unsupported if Entity.LogicalName isn't supported`` () =
        let entity = Entity(AccountMetadata.EntityLogicalName) |> Some
        let result = RatingLogic.getEntityType entity

        Assert.StrictEqual(SupportedEntityType.Unsupported, result)

    [<Fact>]
    let ``RatingLogic.getEntityType returns Opportunity if Entity.LogicalName equals "opportunity"`` () =
        let entity = Entity(OpportunitytMetadata.EntityLogicalName) |> Some
        let result = RatingLogic.getEntityType entity

        Assert.StrictEqual(SupportedEntityType.Opportunity entity.Value, result)

    [<Fact>]
    let ``RatingLogic.getEntityType returns Lead if Entity.LogicalName equals "lead"`` () =
        let entity = Entity(LeadMetadata.EntityLogicalName) |> Some
        let result = RatingLogic.getEntityType entity

        Assert.StrictEqual(SupportedEntityType.Lead entity.Value, result)

    [<Fact>]
    let ``RatingLogic.updateTarget updates Attributes collection for supported entity type`` () =
        let entity = Entity(OpportunitytMetadata.EntityLogicalName)
        let expectedRating = OptionSetValue(LanguagePrimitives.EnumToValue OpportunitytMetadata.Rating.Hot)
        let result = RatingLogic.updateTarget (SupportedEntityType.Opportunity entity) expectedRating

        Assert.True(result.IsSome)
        let actualRating = result.Value.GetAttributeValue<OptionSetValue>(OpportunitytMetadata.Fields.OpportunityRatingCode)
        Assert.StrictEqual(actualRating, expectedRating)

    [<Fact>]
    let ``RatingLogic.updateTarget return None for unsupported entity type`` () =
        let rating = OptionSetValue(LanguagePrimitives.EnumToValue OpportunitytMetadata.Rating.Cold)
        let result = RatingLogic.updateTarget (SupportedEntityType.Unsupported) rating

        Assert.True(result.IsNone)