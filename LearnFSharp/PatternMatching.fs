module PatternMatching

// define a "union" of two different alternatives
type Result<'a, 'b> = 
    | Success of 'a // 'a means generic type. The actual type
    // will be determined when it is used.
    | Failure of 'b // generic failure type as well    

type FileErrorReason = 
    | FileNotFound of string
    | UnauthorizedAccess of string * System.Exception

let performActionOnFile action filePath = 
    try 
        //open file, do the action and return the result
        use sr = new System.IO.StreamReader(filePath : string)
        let result = action sr //do the action to the reader
        sr.Close()
        Success(result) // return a Success
    with // catch some exceptions and convert them to errors
    | :? System.IO.FileNotFoundException as ex -> Failure(FileNotFound filePath)
    | :? System.Security.SecurityException as ex -> Failure(UnauthorizedAccess(filePath, ex))

// a function in the middle layer
let middleLayerDo action filePath = 
    let fileResult = performActionOnFile action filePath
    // do some stuff
    fileResult //return

// a function in the top layer
let topLayerDo action filePath = 
    let fileResult = middleLayerDo action filePath
    // do some stuff
    fileResult //return

/// get the first line of the file
let printFirstLineOfFile filePath = 
    let fileResult = topLayerDo (fun fs -> fs.ReadLine()) filePath
    match fileResult with
    | Success result -> 
        // note type-safe string printing with %s
        printfn "first line is: '%s'" result
    | Failure reason -> 
        match reason with // must match EVERY reason
        | FileNotFound file -> printfn "File not found: %s" file
        | UnauthorizedAccess(file, _) -> printfn "You do not have access to the file: %s" file

printFirstLineOfFile "test.txt"

