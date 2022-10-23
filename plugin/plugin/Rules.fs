namespace OpportunityManagement

open System
open Microsoft.Xrm.Sdk
open Microsoft.FSharp.Linq.NullableOperators

module RatingRules =

    let DetermineOpportunityRating (repository: Repositories.IAccountRepository) (opportunity: Entity): OptionSetValue =
        let parentAccountReference = opportunity.GetAttributeValue<EntityReference>(OpportunitytMetadata.Fields.ParentAccount)

        if parentAccountReference <> null
        then
            let parentAccount = repository.GetById parentAccountReference.Id [AccountMetadata.Fields.NumberOfEmployees]
            let numberOfEmployees = parentAccount.GetAttributeValue<Nullable<int>>(AccountMetadata.Fields.NumberOfEmployees)
            let rating =
                match numberOfEmployees with
                | numberOfEmployees when numberOfEmployees ?>= 100 -> new OptionSetValue(int OpportunitytMetadata.Rating.Hot)
                | numberOfEmployees when numberOfEmployees ?>= 10 -> new OptionSetValue(int OpportunitytMetadata.Rating.Warm)
                | numberOfEmployees when numberOfEmployees ?>= 1 -> new OptionSetValue(int OpportunitytMetadata.Rating.Cold)
                | _ -> null
            rating
        else
            null

    let DetermineLeadRating (repository: Repositories.IAccountRepository) (opportunity: Entity): OptionSetValue =
        let parentAccountReference = opportunity.GetAttributeValue<EntityReference>(OpportunitytMetadata.Fields.ParentAccount)

        if parentAccountReference <> null
        then
            let parentAccount = repository.GetById parentAccountReference.Id [AccountMetadata.Fields.NumberOfEmployees]
            let numberOfEmployees = parentAccount.GetAttributeValue<Nullable<int>>(AccountMetadata.Fields.NumberOfEmployees)
            let rating =
                match numberOfEmployees with
                | numberOfEmployees when numberOfEmployees ?>= 1000 -> new OptionSetValue(int LeadMetadata.Rating.Hot)
                | numberOfEmployees when numberOfEmployees ?>= 100 -> new OptionSetValue(int LeadMetadata.Rating.Warm)
                | numberOfEmployees when numberOfEmployees ?>= 10 -> new OptionSetValue(int LeadMetadata.Rating.Cold)
                | _ -> null
            rating
        else
            null