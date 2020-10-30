module App

open Browser
open Elmish
open Feliz
open Feliz.UseElmish

type Message =
    | NoOp
    | Increment
    | Decrement

type State = { Count: int }

let init count = { Count = count }, Cmd.none

let update msg state =
    match msg with
    | NoOp -> state, Cmd.none
    | Increment ->
        { state with Count = state.Count + 1 },
        Cmd.OfFunc.perform (fun () -> console.log "Increment") () (fun () -> NoOp)
    | Decrement ->
        { state with Count = state.Count - 1 },
        Cmd.OfFunc.perform (fun () -> console.log "Decrement") () (fun () -> NoOp)

[<ReactComponent>]
let button (props: {| label: string
                      onClick: Types.MouseEvent -> unit |}) =
    Html.button [ prop.text props.label
                  prop.className [ Tw.border
                                   Tw.rounded
                                   Tw.px2 ]
                  prop.onClick props.onClick ]

[<ReactComponent>]
let counter (initialCount: int) =
    let state, dispatch =
        React.useElmish (init initialCount, update)

    Html.div [ Html.h1 [ prop.className [ Tw.textGreen500 ]
                         prop.text state.Count ]
               button
                   {| label = "+"
                      onClick = fun _ -> dispatch Increment |}
               button
                   {| label = "-"
                      onClick = fun _ -> dispatch Decrement |} ]

[<ReactComponent(exportDefault = true)>]
let app () = counter 42
