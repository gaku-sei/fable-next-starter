namespace App

open Api
open Browser
open Elmish
open Feliz
open Feliz.UseElmish

type AsyncData<'A> =
    | Init
    | Loading
    | Complete of 'A

type AsyncResult<'Ok, 'Error> = AsyncData<Result<'Ok, 'Error>>

type Message =
    | NoOp
    | Increment
    | Decrement
    | FetchContinents
    | ContinentsFetched of Continents.Continent list
    | ContinentsFetchedFailure of exn

type State =
    { Continents: AsyncResult<Continents.Continent list, string>
      Count: int }

module App =
    let init count =
        { Continents = Init; Count = count }, Cmd.ofMsg FetchContinents

    let update msg state =
        match msg with
        | NoOp -> state, Cmd.none
        | Increment ->
            { state with Count = state.Count + 1 },
            Cmd.OfFunc.perform (fun () -> console.log "Increment") () (fun () -> NoOp)
        | Decrement ->
            { state with Count = state.Count - 1 },
            Cmd.OfFunc.perform (fun () -> console.log "Decrement") () (fun () -> NoOp)
        | FetchContinents ->
            { state with Continents = Loading },
            Cmd.OfAsync.either (fun () ->
                async {
                    let! continents = Api.Client().Continents()

                    return match continents with
                           | Ok { continents = continents } -> continents
                           | Error _ -> failwith "Fetch error"
                }) () ContinentsFetched ContinentsFetchedFailure
        | ContinentsFetched continents ->
            { state with
                  Continents = Complete <| Ok continents },
            Cmd.none
        | ContinentsFetchedFailure exn ->
            { state with
                  Continents = Complete <| Error exn.Message },
            Cmd.none

    [<ReactComponent>]
    let button (props: {| label: string
                          onClick: Types.MouseEvent -> unit |}) =
        Html.button [ prop.text props.label
                      prop.className [ Tw.border
                                       Tw.rounded
                                       Tw.``px-2`` ]
                      prop.onClick props.onClick ]

    [<ReactComponent>]
    let counter (props: {| increment: Types.MouseEvent -> unit
                           decrement: Types.MouseEvent -> unit
                           value: int |}) =

        Html.div [ Html.h1 [ prop.className [ Tw.``text-green-500`` ]
                             prop.text props.value ]
                   button
                       {| label = "+"
                          onClick = props.increment |}
                   button
                       {| label = "-"
                          onClick = props.decrement |} ]

    [<ReactComponent>]
    let continents continents =
        Html.div [ Html.h1 "Continents"
                   Html.div [ match continents with
                              | Init
                              | Loading -> Html.div "Loading..."
                              | Complete (Error error) -> Html.div ("An error occured: " + error)
                              | Complete (Ok (continents: Continents.Continent list)) ->
                                  Html.div
                                      (continents
                                       |> List.map (fun { name = name } -> Html.div name)) ] ]

    [<ReactComponent(exportDefault = true)>]
    let app () =
        let state, dispatch = React.useElmish (init 42, update)

        Html.div [ prop.children [ counter
                                       {| increment = fun _ -> dispatch Increment
                                          decrement = fun _ -> dispatch Decrement
                                          value = state.Count |}
                                   continents state.Continents ] ]
