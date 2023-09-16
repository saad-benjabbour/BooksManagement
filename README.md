# Books Management ASP.NET CORE Reference Application
.NET CORE REST API that manage different books, video tapes and membership of patrons of library

 > **_NOTE:_** <br>
	The master branch is currently running ASP.NET CORE 5.0 <br>
Multi targeted: net7.0; net6.0; net5.0; netcoreapp3.1; netcoreapp3.0; 

## Functionalities
* Library Assets <br>
1- Add Assets (Books, videos) <br>
2- Retrieve all assets that the library have <br>
3- Retrieve all the data related the asset such as Author or Director, DeweyIndex, Isbn, Title and Type <br>
4- Remove a specific library Asset

* Checkouts <br>
1- Placing checkouts on a library asset<br>
2- Getting the checkout history on a library asset <br>
3- Remove checkouts for a library asset<br>
4- close the checkout history<br>
5- Checking a library Asset<br>
6- placing hold on a library Asset <br>
7- Get the current Checkout of a patron<br>
8- Get checkout by asset<br>
9- Retrieving all the current holds and checkouts on a library asset for a patron<br>
10- Mark a library asset either Found or Lost<br>

* Patron <br>
1- Add a patron to the library<br>
2- Retrieve the patrons that are currently subscribed to the library <br>
3- Retrieve all the holds and checkouts that are placed by a patron on a specefic library asset <br>

# Authorization and authentication process
JSON Web Token (JWT) is an open standard (RFC 7519) that defines a compact and self-contained way for securely transmitting information between parties as a JSON object. This information can be verified and trusted because it is digitally signed. JWTs can be signed using a secret (with the HMAC algorithm) or a public/private key pair using RSA or ECDSA.
[Read About it](https://jwt.io/introduction) <br>

## Architecture of the app
![title](images/architecture_app.png)
## Database representation of the app
![title](images/database_app.png)

# How to test the API
* By using PostMan:
	[Read about it](https://www.postman.com/)
* By using Swagger:
	Its pre-configured in this project as soon as you execute the app swagger will be prompted to you in your browser [Read About it](https://swagger.io/)<br>
<div style="text-align:center">
	<img src="images/postman.png" style="margin-right:50px; width="180px"; height="180px"">
	<img src="images/swagger.png" style="margin-top:80px">
</div>






