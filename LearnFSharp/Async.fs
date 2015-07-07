module Async

open System
open System.Threading
open System.IO
open System.Net
open Microsoft.FSharp.Control.CommonExtensions

let userTimerWithAsync = 
    let timer = new System.Timers.Timer(2000.0)
    let timerEvent = Async.AwaitEvent(timer.Elapsed) |> Async.Ignore
    printfn "Waiting for timer at %O" DateTime.Now.TimeOfDay
    timer.Start()
    printfn "Doing something useful while waiting for event"
    Async.RunSynchronously timerEvent
    printfn "Timer ticked at %O" DateTime.Now.TimeOfDay

userTimerWithAsync

let fileWriteWithAsync = 
    use stream = new System.IO.FileStream("c:\\users\\richard\\test.txt", System.IO.FileMode.Create)
    printfn "Starting async write"
    let asyncResult = stream.BeginWrite(Array.empty, 0, 0, null, null)
    let async = Async.AwaitIAsyncResult(asyncResult) |> Async.Ignore
    printfn "Doing something useful while waiting for write to complete"
    Async.RunSynchronously async
    printfn "Async write completed"

fileWriteWithAsync

//you can write an async workflow using the async keyword and brackets
let sleepWorkflow = 
    async { 
        printfn "Starting sleep workflow at %O" DateTime.Now.TimeOfDay
        do! Async.Sleep 2000
        printfn "Finished sleep workflow at %O" DateTime.Now.TimeOfDay
    }

Async.RunSynchronously sleepWorkflow

//you can nest workflows
let nestedWorkflow = 
    async { 
        printfn "Starting parent"
        let! childWorkflow = Async.StartChild sleepWorkflow
        // give the child a chance and then keep working
        do! Async.Sleep 100
        printfn "Doing something useful while waiting "
        // block on the child
        let! result = childWorkflow
        // done
        printfn "Finished parent"
    }

// run the whole workflow
Async.RunSynchronously nestedWorkflow

let testLoop = 
    async { 
        for i in [ 1..100 ] do
            // do something
            printf "%i before.." i
            // sleep a bit 
            do! Async.Sleep 10
            printfn "..after"
    }

//cancellation token sources are supported as a parameter for any async workflow and Async action in the workflow
//checks the token in the function above its Async.sleep
let cancellationSource = new CancellationTokenSource()

// start the task, but this time pass in a cancellation token
Async.Start(testLoop, cancellationSource.Token)
// wait a bit
Thread.Sleep(200)
// cancel after 200ms
cancellationSource.Cancel()

//run workflows in parallel
let sleepWorkflowMs ms = 
    async { 
        printfn "%i ms workflow started" ms
        do! Async.Sleep ms
        printfn "%i ms workflow finished" ms
    }

let sleep1 = sleepWorkflowMs 1000
let sleep2 = sleepWorkflowMs 2000

[ // run them in parallel
  sleep1; sleep2 ]
|> Async.Parallel
|> Async.RunSynchronously
|> ignore

//realistic example of parallel work
let fetchUrl url = 
    let req = WebRequest.Create(Uri(url))
    use resp = req.GetResponse()
    use stream = resp.GetResponseStream()
    use reader = new IO.StreamReader(stream)
    let html = reader.ReadToEnd()
    printfn "finished downloading %s" url

let fetchUrlAsync url = 
    async { 
        let req = WebRequest.Create(Uri(url))
        use! resp = req.AsyncGetResponse() // new keyword "use!"  
        use stream = resp.GetResponseStream()
        use reader = new IO.StreamReader(stream)
        let html = reader.ReadToEnd()
        printfn "finished downloading %s" url
    }

let sites = 
    [ "http://www.bing.com"; "http://www.google.com"; "http://www.microsoft.com"; "http://www.amazon.com"; 
      "http://www.yahoo.com" ]

sites
|> List.map fetchUrl sites
|> ignore

sites
|> List.map fetchUrlAsync 
|> Async.Parallel 
|> Async.RunSynchronously 
|> ignore
