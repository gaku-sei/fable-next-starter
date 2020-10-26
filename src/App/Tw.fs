[<AutoOpen>]
module Tw

open Zanaptak.TypedCssClasses

[<AutoOpen>]
type Tw = CssClasses<source="../../public/tailwind.min.css", naming=Naming.CamelCase>
