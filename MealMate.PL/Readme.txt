------- API DOCUMENTATION -------
Rotute("/A") = ALL API that return A
Route("/customers") = All API that return customers

---- USERS ----
GET "/users/login/:email/:password" : "Login",

---- CUSTOMERS ----
CustomerDTO
	{
	   Guid CustomerID
	   string Address
	   string FName
	   string LName
	   string CPhone
	   string CEmail
	   decimal TotalMoneySpent
	}

GET "/customers" : "Get all customers"
	=> Return  StatusCode(...)	+  List<CustomerDTO>

GET "/customers/:id" : "Get customer by customer id"
	=> Return  StatusCode(...)	+  CustomerDTO

GET "/customers/lastid" : "Get last customer id"
	=> Return  StatusCode(...)  +  { Guid CustomerId }

GET "/customers/rank/:customerid" : "Get customer rank by customer id",
	=> RETURN  StatusCode(...)  +  { string Rank }

PATCH "/customers/:id/" : "Update customer information",
	From request body needs to have one of them:
		- Address
		- FName
		- LName
		- CPhone
	=> Return  StatusCode(...)  +  CustomerDTO

PATCH "/customers/totalmoney/:id/:money" : "Add total money spent by customer id",
	=> Return  StatusCode(...)  + 
		{
			Guid CustomerID
			decimal TotalMoneySpent
			string Rank
			int FortuneChance
		}

POST "/customers" : "Create new customer"
	From request body needs to have:
		- CustomerID
		- Address (optional)
		- FName
		- LName
		- CPhone
		- CEmail
		- Password
	=> Return  StatusCode(...)  +  CustomerDTO

---- STORE MANAGER ----
ManagerDto
	{
        Guid EmployeeID
        string FName
        double Salary
        string LName
        string Address
        string Email
        Guid StoreID
        string EPhone
    }

GET "/storemanagers" : "Get all store managers"
	=> Return  StatusCode(...)  +  List<ManagerDto>

GET "/storemanagers/:id" : "Get store manager by store manager id"
	=> Return  StatusCode(...)  +  ManagerDto

POST "/storemanagers" : "Create new store manager"
	From request body needs to have:
		- FName
		- Salary
		- LName
		- Address
		- Email
		- StoreID
		- EPhone
		- Password
	=> Return  StatusCode(...)  +  ManagerDto

PATCH "/storemanagers/:id/" : "Update store manager information",
	From request body needs to have one of them:
		- FName
		- Salary
		- LName
		- Address
		- Phone
	=> Return  StatusCode(...)  +  ManagerDto

DELETE "/storemanagers/:id" : "Delete store manager by store manager id",
	=> Return  StatusCode(...) 

---- SHIPPER ----
ShipperDto
	{
		Guid ShipperID
		string Address
		string FName
		string LName
		string SPhoneNo
		string SEmail
		double Capacity
	}

GET "/shippers" : "Get all shippers"
	=> Return  StatusCode(...)  +  List<ShipperDto>

GET "/shippers/:id" : "Get shipper by shipper id"
	=> Return  StatusCode(...)  +  ShipperDto

GET "/shippers/:phone" : "Get shipper by phone number"
	=> Return  StatusCode(...)  +  ShipperDto

PATCH "/shippers/:id/:capacity" : "Update shipper capacity"
	=> Return  StatusCode(...)  +  ShipperDto

PATCH "/shippers/:id/" : "Update shipper information",
	From request body needs to have one of them:
		- Address
		- FName
		- LName
		- SPhone
	=> Return  StatusCode(...)  +  ShipperDto

POST "/shippers" : "Create new shipper"
	From request body needs to have:
		- Address
		- FName
		- LName
		- SPhone
		- SEmail
		- Password
	=> Return  StatusCode(...)  +  ShipperDto

DELETE "/shippers/:id" : "Delete shipper by shipper id",
	=> Return  StatusCode(...)

---- PROMOTION ----
ProductPromotionDto
	{
		Guid PromotionID
		string Name
		decimal Discount
		string Description
		DateTime StartDay
		DateTime EndDay
		Guid ProductID
	}

BillPromotionDto
	{
		Guid PromotionID
		string Name
		decimal Discount
		string Description
		DateTime StartDay
		DateTime EndDay
		double ApplyPrice
	}

CategoryPromotionDto
	{
		Guid PromotionID
		string Name
		decimal Discount
		string Description
		DateTime StartDay
		DateTime EndDay
		string Category
	}

CustomerPromotionDto
	{
		Guid PromotionID
		string Name
		decimal Discount
		string Description
		DateTime StartDay
		DateTime EndDay
		Guid ProductId
		Guid CustomerId
	}

GET "/promotions/product" : "Get all product promotions",
GET "/promotions/product/promotion/:promotionid" : "Get all product promotions by promotion id",
GET "/promotions/product/:productid" : "Get promotion by product id
POST "/promotions/product/:productid" : "Create new product promotion"
	From request body needs to have:
		- Guid PromotionID
		- string Name
		- decimal Discount
		- string Description
		- DateTime StartDay
		- DateTime EndDay
	=> Return  StatusCode(...)  +  ProductPromotionDto
DELETE "/promotions/product/:id" : "Delete product promotion by promotion id",

GET "/promotions/bill" : "Get all bill promotions"
GET "/promotions/bill/promotion/:promotionid" : "Get all bill promotions by promotion id"
GET "/promotions/bill/:billid" : "Get promotion by bill id"
POST "/promotions/bill/:applyprice" : "Create new bill promotion"
	From request body needs to have:
		- Guid PromotionID
		- string Name
		- decimal Discount
		- string Description
		- DateTime StartDay
		- DateTime EndDay
	=> Return  StatusCode(...)  +  BillPromotionDto
DELETE "/promotions/bill/:id" : "Delete bill promotion by promotion id",

GET "/promotions/category" : "Get all category promotions",
GET "/promotions/category/promotion/:promotionid" : "Get all category promotions by promotion id",
GET "/promotions/category/:category" : "Get promotion by category",
POST "/promotions/category/:category" : "Create new category promotion"
	From request body needs to have:
		- string Name
		- decimal Discount
		- string Description
		- DateTime StartDay
		- DateTime EndDay
	=> Return  StatusCode(...)  +  CategoryPromotionDto
DELETE "/promotions/category/:id" : "Delete category promotion by promotion id",

GET "/promotions/customer/:customerid" : "Get all customer promotions by customer id",
GET "/promotions/customer/promotion/:promotionid" : "Get all customer promotions by promotion id",
POST "/promotions/customer/:productId" : "Create new customer promotion"
	From request body needs to have:
		- Guid PromotionID
		- string Name
		- decimal Discount
		- string Description
		- DateTime StartDay
		- DateTime EndDay
	=> Return  StatusCode(...)  +  CustomerPromotionDto

POST "/promotions/customer/:promotionid/:customerid" : "Assign customer promotion to customer"
	=> Return  StatusCode(...)  +  CustomerPromotionDto

DELETE "/promotions/customer/:id" : "Delete customer promotion by promotion id",

---- ADMIN ----


---- PRODUCTS ----
Return 
	StatusCode(...) + 
	{
		Guid ProductID
		string Category
		string Description
		string PName
		double Price
		decimal Discount
		double DiscountedPrice
		double Weight
		string ImageURL
	}

GET "/products" : "Get all products",
GET "/products/:id" : "Get product by product id",
GET "/products/:category" : "Get all products by category",
GET "/products/promotion/:promotionId" : "Get all products by promotion id",
GET "/products/promotion" : "Get all products with promotion",
GET "/products/store/:storeid" : "Get all products by store id",
GET "/products/atstore/:productid" : "Get product information at store by product id",
GET "/products/transaction/:transactionid" : "Get all products by transaction id",
GET "/products/top5products/:year" : "Get top 5 revenue product by year",
POST "/products" : "Create new product"
	From request body needs to have:
		- Category
		- Description
		- PName
		- Price (double)
		- Weight (double)
		- ImageURL
PATCH "/products/:id/" : "Update product information",
	From request body needs to have one of them:
		- Category
		- Description
		- PName
		- Price (double)
		- Weight (double)
		- ImageURL
PATCH "/products/addtostore/:productid/:storeid/:amount" : "Update product stock",
DELETE "/products/:id" : "Delete product by product id",

---- STORES ----
RETURN 
	StatusCode(...) + 
	{
		Guid StoreID
		string Name
		string OpeningDate
		string ContactInfo
		string Location
	}

GET "/stores" : "Get all stores",
GET "/stores/:id" : "Get store by store id",

---- BILL ----
GET "/transactions" : "Get all bills", 
GET "/transactions/customer/:customerid" : "Get all bills by customer id",
RETURN 
	StatusCode(...) + 
	{
        public required Guid TransactionId
        public required Guid CustomerID
        public required Guid StoreID
        public Guid ShipperID
        public required DateTime DateAndTime
        public required DeliveryStatus DeliveryStatus
        public required double TotalPrice
        public required int TotalWeight
    }

GET "/transactions/:transactionid" : "Get full bill by bill id",
RETURN 
	StatusCode(...) + 
	{
		public required Guid TransactionId
		public required Guid CustomerID
		public required Guid StoreID
		public Guid ShipperID
		public required DateTime DateAndTime
		public required DeliveryStatus DeliveryStatus
		public required double TotalPrice
		public required int TotalWeight
		public required List<INCLUDE> Includes
	}
		public class IncludeDto
		{
			public required Guid TransactionID
			public required Guid ProductID
			public required string ProductName
			public required double ProductPrice
			public required int NumberOfProductInBill
			public required double SubTotal
		}

GET "/transactions/last/:customerid" : "Get last bill id by customer id",
POST "/transactions" : "Create new bill"
	From request body needs to have:
		- PaymentMethod
		- DateAndTime
		- CustomerID
		- StoreID
		- TotalPrice
		- TotalWeight
		- List of INCLUDE
			+ ProductID
			+ Quantity
			+ SubTotal

PATCH "/transactions/:transactionid/:shipperid" : "Assign shipper to bill"
	=> Return  StatusCode(...)  +  TransactionDto

PATCH "/transactions/:transactionid/:status" : "Update delivery status of bill"
	=> Return  StatusCode(...)  +  DeliveryStatus

-------- EXAMPLE USAGE --------
API: GET "/promotions/product" : "Get all product promotions",
import axios from 'axios';

const getPromotions = async () => {
    try {
        const response = await axios.get('https://yourapiurl.com/promotions/product');
        
        if (response.status === 200) {
            // Handle successful data retrieval
            console.log("Promotions:", response.data);
            return response.data; // Returns promotions data if needed
        }
    } catch (error) {
        if (error.response) {
            // Server responded with a status code out of 2xx range
            console.error("Error:", error.response.data.message);
            alert(`Error: ${error.response.data.message}`);
        } else if (error.request) {
            // No response received
            console.error("Error: No response received from the server.");
            alert("Error: No response received from the server.");
        } else {
            // Other error scenarios (e.g., configuration issues)
            console.error("Error:", error.message);
            alert(`Error: ${error.message}`);
        }
    }
};
