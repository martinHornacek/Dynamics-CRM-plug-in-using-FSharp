namespace OpportunityManagement

open Microsoft.Xrm.Sdk

type RatingPlugin(secureConfiguration: string, unsecureConfiguration: string) =
    interface IPlugin with
        member this.Execute serviceProvider =
            let context = Common.getContext serviceProvider
            let service = Common.getInitiatingUserService serviceProvider
            RatingLogic.SetRating context service