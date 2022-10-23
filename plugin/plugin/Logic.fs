namespace OpportunityManagement

open Microsoft.Xrm.Sdk

module RatingLogic =
    open Repositories

    type SupportedEntityType =
        | Opportunity of Entity
        | Lead of Entity
        | Unsupported

    let updateTarget (entityType: SupportedEntityType) (rating: OptionSetValue): Entity option =
        match entityType with
        | Opportunity entity ->
            entity.Attributes.[OpportunitytMetadata.Fields.OpportunityRatingCode] <- rating
            Some entity
        | Lead entity ->
            entity.Attributes.[LeadMetadata.Fields.LeadQualityCode] <- rating
            Some entity
        | Unsupported -> None

    let determineRatingForEntity (service: IOrganizationService) (entityType: SupportedEntityType): (SupportedEntityType * OptionSetValue option) =
        let repository = (new AccountRepository(service))
        match entityType with
        | Opportunity entity -> entityType, Some (RatingRules.DetermineOpportunityRating repository entity)
        | Lead entity -> entityType, Some (RatingRules.DetermineLeadRating repository entity)
        | Unsupported -> entityType, None

    let getEntityType (entity: Entity option): SupportedEntityType =
        match entity with
        | Some entity when entity.LogicalName = OpportunitytMetadata.EntityLogicalName -> Opportunity entity
        | Some entity when entity.LogicalName = LeadMetadata.EntityLogicalName -> Lead entity
        | _ -> Unsupported

    let SetRating (context: IPluginExecutionContext) (service: IOrganizationService): unit =
        let entityType, rating =
            Some context
            |> Common.filterOnCreate
            |> Common.filterOnPreOperation
            |> Common.getTarget
            |> getEntityType
            |> determineRatingForEntity service

        updateTarget entityType rating.Value |> ignore