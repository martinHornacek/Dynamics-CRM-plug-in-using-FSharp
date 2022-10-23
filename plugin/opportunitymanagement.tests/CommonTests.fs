namespace OpportunityManagement.Tests

open Foq
open Microsoft.Xrm.Sdk
open OpportunityManagement
open System
open Xunit

module CommonTests =

    [<Fact>]
    let ``Common.filterOnCreate returns None if IPluginExecutionContext.MessageName does not equal "Create"`` () =
        let mock = Mock<IPluginExecutionContext>()
                    .Setup(fun ctx -> <@ ctx.MessageName @>)
                    .Returns("Delete")
                    .Create() |> Some
        let result = Common.filterOnCreate mock

        Assert.StrictEqual(None, result)

    [<Fact>]
    let ``Common.filterOnCreate returns context if IPluginExecutionContext.MessageName equals "Create"`` () =
        let mock = Mock<IPluginExecutionContext>()
                    .Setup(fun ctx -> <@ ctx.MessageName @>)
                    .Returns("Create")
                    .Create() |> Some
        let result = Common.filterOnCreate mock

        Assert.True(result.IsSome)
        Assert.StrictEqual(mock, result)

    [<Fact>]
    let ``Common.filterOnPreOperation returns None if IPluginExecutionContext.Stage does not equal 20`` () =
        let context = Mock<IPluginExecutionContext>()
                        .Setup(fun ctx -> <@ ctx.Stage @>)
                        .Returns(40)
                        .Create() |> Some
        let result = Common.filterOnPreOperation context

        Assert.True(result.IsNone)

    [<Fact>]
    let ``Common.filterOnPreOperation returns context if IPluginExecutionContext.Stage equals 20`` () =
        let context = Mock<IPluginExecutionContext>()
                        .Setup(fun ctx -> <@ ctx.Stage @>)
                        .Returns(20)
                        .Create() |> Some
        let result = Common.filterOnPreOperation context

        Assert.StrictEqual(context, result)

    [<Fact>]
    let ``Common.getTarget returns Target as Entity from if IPluginExecutionContext.InputParameters if it contains key with name "Target" and is of type Entity`` () =
        let inputParameters = ParameterCollection()
        inputParameters.Add(PluginMetadata.Target, Entity())

        let context = Mock<IPluginExecutionContext>()
                        .Setup(fun ctx -> <@ ctx.InputParameters @>)
                        .Returns(inputParameters)
                        .Create() |> Some
        let result = Common.getTarget context

        Assert.True(result.IsSome)
        Assert.NotNull(result.Value);
        Assert.IsType(typedefof<Entity>, result.Value)

    [<Fact>]
    let ``Common.getTarget returns None if IPluginExecutionContext.InputParameters does not contain key with name "Target"`` () =
        let inputParameters = ParameterCollection()
        let context = Mock<IPluginExecutionContext>()
                        .Setup(fun ctx -> <@ ctx.InputParameters @>)
                        .Returns(inputParameters)
                        .Create() |> Some
        let result = Common.getTarget context

        Assert.True(result.IsNone)

    [<Fact>]
    let ``Common.getTarget throws if IPluginExecutionContext.InputParameters contains key with name "Target" but isn't of type Entity`` () =
        let inputParameters = ParameterCollection()
        inputParameters.Add(PluginMetadata.Target, EntityReference)

        let context = Mock<IPluginExecutionContext>()
                        .Setup(fun ctx -> <@ ctx.InputParameters @>)
                        .Returns(inputParameters)
                        .Create() |> Some

        Assert.Throws<InvalidCastException>(fun _ -> (Common.getTarget context |> ignore))