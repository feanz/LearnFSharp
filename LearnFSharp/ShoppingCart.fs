module ShoppingCart

open System

type CartItem = string

type EmptyCart = NoItems

type ActiveCart = 
    { UnpaidItems : CartItem list }

type PaidForCart = 
    { PaidItems : CartItem list
      Payment : decimal }

type Cart = 
    | Empty of EmptyCart
    | Active of ActiveCart
    | PaidFor of PaidForCart

let addToEmptyState item = Cart.Active { UnpaidItems = [ item ] }

let addToActiveState state itemToAdd = 
    let newList = itemToAdd :: state.UnpaidItems
    Cart.Active { state with UnpaidItems = newList }

let removeFromActiveState state itemToRemove = 
    let newList = state.UnpaidItems |> List.filter (fun i -> i <> itemToRemove)
    match newList with
    | [] -> Cart.Empty NoItems
    | _ -> Cart.Active { state with UnpaidItems = newList }

let payForActiveState state amount = 
    Cart.PaidFor { PaidItems = state.UnpaidItems
                   Payment = amount }

type EmptyCart with
    member this.Add = addToEmptyState

type ActiveCart with
    member this.Add = addToActiveState this
    member this.Remove = removeFromActiveState this
    member this.Pay = payForActiveState this

let addItemToCart cart item = 
    match cart with
    | Empty state -> state.Add item
    | Active state -> state.Add item
    | PaidFor state -> raise (InvalidOperationException("You can't add items to a paid for cart"))

let removeItemFromCart cart item = 
    match cart with
    | Empty state -> 
        raise (InvalidOperationException("You can't remove items from an empty cart"))        
    | Active state -> state.Remove item
    | PaidFor state -> 
        raise (InvalidOperationException("You can't remove items from a paid for cart"))        

let displayCart cart = 
    match cart with
    | Empty state -> printfn "The cart is empty" // can't do state.Items
    | Active state -> printfn "The cart contains %A unpaid items" state.UnpaidItems
    | PaidFor state -> printfn "The cart contains %A paid items. Amount paid: %f" state.PaidItems state.Payment

let payForCart cart amount = 
    match cart with 
    | Empty state -> raise (InvalidOperationException("Can't pay for empty cart"))
    | Active state -> state.Pay amount
    | PaidFor state -> raise (InvalidOperationException("Can't pay for paided for cart"))

type Cart with
    static member NewCart = Cart.Empty NoItems
    member this.Add = addItemToCart this
    member this.Remove = removeItemFromCart this
    member this.Display = displayCart this
    member this.Pay = payForCart this 

let emptyCart = Cart.NewCart
printf "emptyCart="; emptyCart.Display

let cartA = emptyCart.Add "A"
printf "cartA="; cartA.Display

let cartAB = cartA.Add "B"
printf "cartAB="; cartAB.Display

let cartB = cartAB.Remove "A"
printf "cartB="; cartB.Display

let emptyCart2 = cartB.Remove "B"
printf "emptyCart2="; emptyCart2.Display

let emptyCart3 = emptyCart2.Remove "B"    //error
printf "emptyCart3="; emptyCart3.Display

let cartAPaid = 
    match cartA with
    | Empty _ | PaidFor _ -> cartA 
    | Active state -> state.Pay 100m
printf "cartAPaid="; cartAPaid.Display



