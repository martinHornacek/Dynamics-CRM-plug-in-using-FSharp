namespace OpportunityManagement

open System
open Microsoft.Xrm.Sdk
open Microsoft.Xrm.Sdk.Query

module Repositories =

    [<AbstractClass>]
    type Repository(service: IOrganizationService) =

        abstract member Update: (Entity option) -> unit

        default this.Update(entity: Entity option): unit =
            match entity with
            | Some entity -> service.Update(entity)
            | _ -> ()

    type IAccountRepository =
        abstract member GetById: Guid -> string list -> Entity

    type AccountRepository(service: IOrganizationService) =
        inherit Repository(service: IOrganizationService)
        interface IAccountRepository with
            member this.GetById (id: Guid) (attributes: string list): Entity =
                service.Retrieve(AccountMetadata.EntityLogicalName, id, ColumnSet(List.toArray attributes))