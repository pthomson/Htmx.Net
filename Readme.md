# Htmx.Net

![](https://raw.githubusercontent.com/bigskysoftware/htmx/master/www/img/htmx_logo.1.png)

This is a package designed to add server side helper methods for `HttpRequest` and `HttpResponse`. This makes working with [htmx](https://htmx.org/) server-side concepts simpler. You should also consider reading about [Hyperscript](https://hyperscript.org/), an optional companion project for HTMX.

## Htmx Extension Methods

### Getting Started

Install the `Htmx` [NuGet package](https://www.nuget.org/packages/Htmx/) to your ASP.NET Core project.

```console
dotnet add package Htmx
```

### HttpRequest

Using the `HttpRequest`, we can determine if the request was initiated by Htmx on the client.

```c#
httpContext.Request.IsHtmx()
```

This can be used to either return a full page response or a partial page render.

```c#
// in a Razor Page
return Request.IsHtmx()
    ? Partial("_Form", this)
    : Page();
```

We can also retrieve the other header values htmx might set.

```c#
Request.IsHtmx(out var values);
```

Read more about the other header values on the [official documentation page](https://htmx.org/reference/#request_headers).

### HttpResponse

We can set Http Response headers using the `Htmx` extension method, which passes an action and `HtmxResponseHeaders` object.

```c#
Response.Htmx(h => {
    h.PushUrl("/new-url")
     .WithTrigger("cool")
});
```

Read more about the HTTP response headers at the [official documentation site](https://htmx.org/reference/#request_headers).

#### Triggering Client-Side Events

You can trigger client side events with HTMX using the `HX-Trigger` header. Htmx.Net provides a `WithTrigger` helper method to configure one or more events that you wish to trigger.

```c#
Response.Htmx(h => {
    h.WithTrigger("yes")
     .WithTrigger("cool", timing: HtmxTriggerTiming.AfterSettle)
     .WithTrigger("neat", new { valueForFrontEnd: 42, status: "Done!" }, timing: Htmx.TriggerTiming.AfterSwap);
});
```

## Htmx.TagHelpers

### Getting Started

Install the `Htmx.TagHelpers` [NuGet package](https://www.nuget.org/packages/Htmx.TagHelpers/) to your ASP.NET Core project. Targets .NET Core 3.1+ projects.

```console
dotnet add package Htmx.TagHelpers
```

Make the Tag Helpers available in your project by adding the following line to your `_ViewImports.cshtml`:

```razor
@addTagHelper *, Htmx.TagHelpers
```

You'll generally need URL paths pointing back to your ASP.NET Core backend. Luckily, `Htmx.TagHelpers` mimics the url generation included in ASP.NET Core. This makes linking HTMX with your ASP.NET Core application a seamless experience.

```html
<div hx-target="this">
    <button hx-get
            hx-page="Index"
            hx-page-handler="Snippet"
            hx-swap="outerHtml">
        Click Me (Razor Page w/ Handler)
    </button>
</div>

<div hx-target="this">
    <button hx-get
            hx-controller="Home"
            hx-action="Index"
            hx-route-id="1">
        Click Me (Controller)
    </button>
</div>

<div hx-target="this">
    <button hx-post
            hx-route="named">
        Click Me (Named)
    </button>
</div>
```

### Htmx.Config

An additional `htmx-config` tag helper is included that can be applied to a `meta` element in your page's `head` that makes creating HTMX configuration simpler. For example, below we can set the `historyCacheSize`, default `indicatorClass`, and whether to include ASP.NET Core's anti-forgery tokens as an additional element on the HTMX configuration.

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta name="htmx-config" 
          historyCacheSize="20"
          indicatorClass="htmx-indicator"
          includeAspNetAntiforgeryToken="true"
          />
    <!-- additional elements... -->
</head>
```

The resulting HTML will be.

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta name="htmx-config" content='{"indicatorClass":"htmx-indicator","historyCacheSize":20,"antiForgery":{"formFieldName":"__RequestVerificationToken","headerName":"RequestVerificationToken","requestToken":"<token>"}}' />
    <!-- additional elements... -->
</head>
```

### HTMX and Anti-forgery Tokens

You can set the attribute `includeAspNetAntiforgerToken` on the `htmx-config` element. Then you'll need to include this additional JavaScript in your web application.

```javascript
document.addEventListener("htmx:configRequest", (evt) => {
    let httpVerb = evt.detail.verb.toUpperCase();
    if (httpVerb === 'GET') return;

    let antiForgery = htmx.config.antiForgery;

    if (antiForgery) {

        // already specified on form, short circuit
        if (evt.detail.parameters[antiForgery.formFieldName])
            return;

        if (antiForgery.headerName) {
            evt.detail.headers[antiForgery.headerName]
                = antiForgery.requestToken;
        } else {
            evt.detail.parameters[antiForgery.formFieldName]
                = antiForgery.requestToken;
        }
    }
});
```

You can access the snippet in two ways. The first is to use the `HtmxSnippet` static class in your views.

```
<script>
@Html.Raw(HtmxSnippets.AntiforgeryJavaScript)
</script>
```

A simpler way is to use the `HtmlExtensions` class that extends `IHtmlHelper`.

```
@Html.HtmxAntiforgeryScript()
```

This html helper will result in a `<script>` tag along with the previously mentioned JavaScript. **Note: You can still register multiple event handlers for `htmx:configRequest`, so having more than one is ok.**

Note that if the `hx-[get|post|put]` attribute is on a `<form ..>` tag, the ASP.NET Tag Helpers will add the Anti-forgery Token as an `input` element and you do not need to further configure your requests as above. You could also use [`hx-include`](https://htmx.org/attributes/hx-include/) pointing to a form, but this all comes down to a matter of preference.


## License

Copyright © 2021 Khalid Abuhakmeh

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
