(*
Slang of the day - Toy application to look up the meaning of slang expressions

Copyright 2023 Salvatore ISAJA. All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice,
this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
this list of conditions and the following disclaimer in the documentation
and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED THE COPYRIGHT HOLDER ``AS IS'' AND ANY EXPRESS
OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN
NO EVENT SHALL THE COPYRIGHT HOLDER BE LIABLE FOR ANY DIRECT,
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*)
open System
open System.IO
open System.Linq
open System.Net.Http
open System.Net.Http.Json
open System.Text.Json

type Definition = { Definition: string }
type DefinitionList = { List: Definition list }

let terms = File.ReadAllLines("terms.txt")
let triedTerms = File.ReadAllLines("tried-terms.txt").ToHashSet()

let rec pickTerm () =
    let index = Random().Next(terms.Length)
    let term = terms[index]
    if triedTerms.Add(term) then term else pickTerm ()

let rec defineTerm () =
    let term = pickTerm ()
    printfn "# %s\n" term
    let definition =
        async { 
            use client = new HttpClient()
            use request = new HttpRequestMessage(
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://mashape-community-urban-dictionary.p.rapidapi.com/define?term={Uri.EscapeDataString(term)}"))
            request.Headers.Add("X-RapidAPI-Key", "<SIGN UP FOR AN API KEY>")
            request.Headers.Add("X-RapidAPI-Host", "mashape-community-urban-dictionary.p.rapidapi.com")
            let! httpResponseMessage = client.SendAsync(request) |> Async.AwaitTask
            httpResponseMessage.EnsureSuccessStatusCode() |> ignore
            let! body = httpResponseMessage.Content.ReadFromJsonAsync<DefinitionList>(JsonSerializerOptions(PropertyNamingPolicy = JsonNamingPolicy.CamelCase)) |> Async.AwaitTask
            return body.List |> List.fold (fun s d -> s + "\n\n- " + d.Definition.ReplaceLineEndings(" ")) ""
        } |> Async.RunSynchronously
    if definition.Length > 0 then definition else defineTerm ()

let definition = defineTerm ()
printfn "%s" definition
File.WriteAllLines("tried-terms.txt.part", triedTerms)
File.Move("tried-terms.txt.part", "tried-terms.txt", overwrite=true)
