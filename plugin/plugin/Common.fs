namespace OpportunityManagement

open System
open Microsoft.Xrm.Sdk

module Common =

    let getContext (serviceProvider: IServiceProvider): IPluginExecutionContext =
        serviceProvider.GetService typedefof<IPluginExecutionContext> :?> IPluginExecutionContext

    let getInitiatingUserService (serviceProvider: IServiceProvider): IOrganizationService =
        let factory = serviceProvider.GetService typedefof<IOrganizationServiceFactory> :?> IOrganizationServiceFactory
        let context = getContext serviceProvider
        factory.CreateOrganizationService(Nullable<Guid>(context.UserId))

    let getSystemUserService (serviceProvider: IServiceProvider): IOrganizationService =
        let factory = serviceProvider.GetService typedefof<IOrganizationServiceFactory> :?> IOrganizationServiceFactory
        factory.CreateOrganizationService(Nullable<Guid>())

    let getTarget (context: IPluginExecutionContext option): Entity option =
        match context with
        | Some context when context.InputParameters.ContainsKey(PluginMetadata.Target) -> Some (context.InputParameters.[PluginMetadata.Target] :?> Entity)
        | _ -> None

    let filterOnCreate (context: IPluginExecutionContext option): IPluginExecutionContext option =
        match context with
        | Some context when context.MessageName = PluginMessageName.Create -> Some context
        | _ -> None

    let filterOnPreOperation (context: IPluginExecutionContext option): IPluginExecutionContext option =
        match context with
        | Some context when context.Stage = PluginStage.PreOperation -> Some context
        | _ -> None