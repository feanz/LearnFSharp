module Sorting


(*
If the list is empty, there is nothing to do.
Otherwise: 
  1. Take the first element of the list
  2. Find all elements in the rest of the list that 
      are less than the first element, and sort them. 
  3. Find all elements in the rest of the list that 
      are >= than the first element, and sort them
  4. Combine the three parts together to get the final result: 
      (sorted smaller elements + firstElement + 
       sorted larger elements)
*)


//note rec flag for method that are recursive
let rec quicksort list =
   match list with
   | [] ->                            // If the list is empty
        []                            // return an empty list
   | firstElem::otherElements ->      // If the list is not empty     
        let smallerElements =         // extract the smaller ones    
            otherElements             
            |> List.filter (fun e -> e < firstElem) 
            |> quicksort              // and sort them
        let largerElements =          // extract the large ones
            otherElements 
            |> List.filter (fun e -> e >= firstElem)
            |> quicksort              // and sort them
        // Combine the 3 parts into a new list and return it
        List.concat [smallerElements; [firstElem]; largerElements]

