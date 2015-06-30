//modules are similar to namespaces in C# 
module Syntax

//these are value not variables they are immutable 
(*
    Note no semicolons at the end of lines 
    also this is not dynamic typing its type infered
*)
let myInt = 5
let myFloat = 3.34
let myString = "hello"
//lists
let twoToFive = [ 2; 3; 4; 5 ]
let oneToFive = 1 :: twoToFive
let zeroToFive = [ 0; 1 ] @ twoToFive
//functions
//let also defines functions 
let square x = x * x

square 3

//you can be explicit about types as some type inference will happen ints are prefered number value
let add (x : decimal, y : decimal) = x + y

// type inference is againt more than just the initial asignment 
let sumLengths strList = 
    strList
    |> List.map String.length
    |> List.sum

//to define multiline functions you use indentation
let evens list = 
    let isEven x = x % 2 = 0
    List.filter isEven list //no return the last line of a function is the return or its a void function

evens oneToFive

//you can use parens to clarify precidence 
let sumOfEvens = 
    let isEven x = x % 2 = 0
    List.sum (List.filter isEven oneToFive)

//its better to pipe function result this make function compoistion easier also means we can read left to right
let SumOfEvensPiped = 
    oneToFive
    |> List.filter (fun x -> x % 2 = 0)
    |> List.sum //notice the lambda expression defined in the set of pipes

//Pattern matching
// Match..with.. is a supercharged case/switch statement.
let simplePatternMatch = 
    let x = "a"
    match x with
    | "a" -> printfn "x is a"
    | "b" -> printfn "x is b"
    | _ -> printfn "x is something else" // underscore matches anything

// Some(..) and None are wrappers for null but being more explicit there are no nulls in f# 
let validValue = Some(99)
let invalidValue = None
//tuples are defined with commas 
let twoTuple = 1, 2
let threeTuple = "a", 2, true

//record types have named fields
type Person = 
    { First : string
      Last : string }

let person1 = 
    { First = "john"
      Last = "Doe" }

//records have structural equality (are the properties the same)
let person2 = 
    { First = "john"
      Last = "Doe" }

let areEqual = person1 = person2

//custom equality 
[<CustomEquality; NoComparison>]
type ManagerAccount = 
    { Id : int }
    override x.Equals(yobj) = 
        match yobj with
            | :? ManagerAccount as y -> (x.Id = y.Id)
            | _ -> false

let manager1 = { Id = 1 }
let manager2 = { Id = 1 }

let sameManager = manager1 = manager2

// you can deny comparison
[<NoEquality; NoComparison>]
type CustomerAccount = 
    { CustomerAccountId : int }

let x = { CustomerAccountId = 1 }

//x = x       // error!
x.CustomerAccountId = x.CustomerAccountId // no error

//union types have choices
type Temp = 
    | DegreesC of float
    | DegreesF of float

let temp = DegreesF 98.6

//types can be combined recursively in complex ways
type Employee = 
    | Worker of Person
    | Manager of Employee list

let jdoe = 
    { First = "John"
      Last = "Doe" }

let worker = Worker jdoe

//built in pretty print for most types
printfn "%A" jdoe
//print is like console writeline
printfn "Printing an int %i, a float %f, a bool %b" 1 2.0 true
