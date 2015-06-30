module TypeSafe

//Units of measure

// define some measures
[<Measure>] 
type cm

[<Measure>] 
type inches

[<Measure>] 
type feet =
   // add a conversion function
   static member toInches(feet : float<feet>) : float<inches> = 
      feet * 12.0<inches/feet>

// define some values
let meter = 100.0<cm>
let yard = 3.0<feet>

//convert to different measure
let yardInInches = feet.toInches(yard)

// can't mix and match!
//yard + meter (this would be a compiler error)

// now define some currencies
[<Measure>] 
type GBP

[<Measure>] 
type USD

let gbp10 = 10.0<GBP>
let usd10 = 10.0<USD>
//gbp10 + gbp10             // allowed: same currency
//gbp10 + usd10             // not allowed: different currency
//gbp10 + 1.0               // not allowed: didn't specify a currency
//gbp10 + 1.0<_>            // allowed using wildcard