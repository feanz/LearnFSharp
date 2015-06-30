module DownLoadWebPage

//open is like using we are using all the .net BCl
open System.Net
open System
open System.IO

// Fetch the contents of a web page
let fetchUrl callback url =        
    let req = WebRequest.Create(Uri(url)) //you have to create a Uri here or type inference wont work
    use resp = req.GetResponse() //use is what you do when you have an IDisposable it will be disposed when it goes out of scope (Nice!)
    use stream = resp.GetResponseStream() 
    use reader = new IO.StreamReader(stream) //the new keyword is when you want to create and IDisposable
    callback reader url