## Fable + Next.js (+ Tailwind) Starter pack

- [Next.js](https://nextjs.org/) pages are located under `src/pages`
- [F#/Fable](https://fable.io/) projects are all located under `src/`
  - `App`: main entry point
  - _in progress_ `Hooks`: all the hooks
  - _in progress_ `Entities`: values and entities
- [Tailwind](tailwindcss.com/) via the [TypedCssClasses](https://github.com/zanaptak/TypedCssClasses) provider, can be replaced easily

### Getting started:

You will need:

- [.Net Core](https://dotnet.microsoft.com/download) installed (v3.1 as of today, v5 soon to be released)
- Node/Yarn

First, run the `dotnet tool restore` and `dotnet restore` commands to get the `fable` plugin and fetch the .Net dependencies.

Then, you can start using Yarn: `yarn && yarn bootstrap` will install the Node dependencies, and create the css file.

Finally you can run `yarn dev` to compile and watch the F# files, and run Next.js
