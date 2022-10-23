namespace OpportunityManagement

module AccountMetadata =
    let EntityLogicalName = "account"

    module Fields =
        let NumberOfEmployees = "numberofemployees"

module OpportunitytMetadata =

    type Rating = Hot = 1 | Warm = 2 | Cold = 3

    let EntityLogicalName = "opportunity"

    module Fields =
        let ParentAccount = "parentaccountid"
        let OpportunityRatingCode = "opportunityratingcode"

module LeadMetadata =

    type Rating = OpportunitytMetadata.Rating

    let EntityLogicalName = "lead"

    module Fields =
        let LeadQualityCode = "leadqualitycode"

module PluginMetadata =
    let Target = "Target"

module PluginMessageName =
    let Create = "Create"

module PluginStage =
    let PreOperation = 20