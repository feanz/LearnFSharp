module calculatorTests

open Xunit
open FsUnit

[<Fact>]
let ``add 5 and 3 should return 8``() = 
    Calculator.add 5 3 |> should equal 8

[<Fact>]
let ``minus 5 and 3 should return 2``() = 
    Calculator.minus 5 3 |> should equal 2

[<Fact>]
let ``times 5 and 3 should return 15``() = 
    Calculator.times 5 3 |> should equal 15

[<Fact>]
let ``square 5 should return 25``() = 
    Calculator.square 5  |> should equal 25