module ShoppingCartTests


open Xunit
open FsUnit


[<Fact>]
let ``can't remove items from an empty cart``() = 
    let cart = ShoppingCart.Cart.NewCart 
    (fun () -> cart.Remove "" |> ignore) |> should throw typeof<System.InvalidOperationException>

[<Fact>]
let ``can't add items to paid for cart``() = 
    let cart = ShoppingCart.Cart.NewCart 
    let cartA = cart.Add("A");
    let paid = cartA.Pay 100m
    (fun () -> paid.Add "" |> ignore) |> should throw typeof<System.InvalidOperationException>
    
[<Fact>]
let ``can't remove items from a paid for cart``() = 
    let cart = ShoppingCart.Cart.NewCart 
    let cartA = cart.Add("A");
    let paid = cartA.Pay 100m
    (fun () -> paid.Remove "" |> ignore) |> should throw typeof<System.InvalidOperationException>

[<Fact>]
let ``can't pay for empty cart``() = 
    let cart = ShoppingCart.Cart.NewCart         
    (fun () -> cart.Pay 100m |> ignore) |> should throw typeof<System.InvalidOperationException>
