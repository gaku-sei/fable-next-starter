namespace App

open Fable.Core.JsInterop
open Node

type Api =
    static member Client(?headers: Fable.SimpleHttp.Header list) =
        let url = ``process``.env?NEXT_PUBLIC_API_URI
        let headers = defaultArg headers []

        Api.ApiGraphqlClient(url = url, headers = headers)
