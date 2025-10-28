# 🛒 MarketplaceCoreAPI

*"Business & Commerce API for the Marketplace"*

---

## 📌 Project Overview

The **MarketplaceCoreAPI** is responsible for all commerce-related features inside the marketplace ecosystem.  
It enables **product visibility & search**, **cart management**, **order creation**, and **shop owner operations** such as product publishing and order handling.  
Admins manage categories, product approval, and moderation of marketplace content.

This API acts as the **business backbone** of the platform, while identity and permissions are managed by the separate `MarketplaceAuthAPI`.

---

## 🧱 Architecture

The API is built using a **clean layered architecture**:

| Layer | Responsibility |
|------|----------------|
| **MarketplaceCoreAPI** | Controllers, routing, input/output handling |
| **BLL (Business Layer)** | Business rules, validations, orchestration |
| **DAL (Data Layer)** | EF Core repositories & persistence |
| **Domain** | Business entities / core models |

Additional principles:
- DTOs separated from Domain Models ✅
- Repository Pattern ✅
- Dependency Injection ✅
- `ServiceResponse<T>` as unified return type ✅

---

## 🛠 Tech Stack

| Technology | Usage |
|-----------|--------|
| .NET 8 / ASP.NET Core 8 | API framework |
| EF Core 8 | ORM / persistence |
| SQL Server | Relational database |
| Stripe | External payment provider (PaymentIntent) |
| SMTP / Mail Service | Order confirmation emails |

---

## 🔐 Roles & Access

> **Role management is fully handled by the `MarketplaceAuthAPI`.**  
> The CoreAPI only validates the JWT token and extracts role claims for authorization.

| Role | Permissions |
|------|-------------|
| **User** | Browse/search products, manage cart, place orders, write reviews/questions |
| **Shop** | Create/update/delete own products, view and process shop orders, answer questions |
| **Admin** | Category management, approve products and reviews |
| **SuperAdmin** | Full system oversight, category/admin-level changes |

---

## 📦 Domain Models

| Model | Purpose |
|------|---------|
| `Cart` | Represents the user's shopping cart |
| `CartItem` | A single product with quantity/variant inside a cart |
| `Category` | Hierarchical category tree (parent/child) |
| `Order` | Represents a full user purchase |
| `OrderItem` | A single ordered product instance |
| `Product` | Main marketplace product entity |
| `ProductCharacteristic` | Technical/specification metadata |
| `ProductMedia` | Product images / video references |
| `DeliveryOption` | Available delivery types per product |
| `KeyValue` | Extendable metadata for products |
| `ProductReview` | User-generated product rating |
| `ProductQuestion` | User enquiry about a product |
| `ProductQuestionAnswer` | Shop's reply to a question |

**Visibility logic**  
Products become publicly visible only when **`IsActive == true` AND `IsApproved == true`**.  
Deletion is **hard delete** and can **only be performed by the product owner (Shop)**.

---

## 🧭 Controllers & Responsibilities

### ✅ MarketplaceController (public & user endpoints)

| Endpoint | Purpose |
|---------|---------|
| `GET /api/Marketplace/GetAllProducts` | Returns all catalog products |
| `GET /GetSimilarProducts` | Suggests similar products by ID |
| `GET /SearchProductsByName` | Name-based product search (+ optional category) |
| `GET /GetFilter` | Retrieves marketplace filters |
| `GET /GetProductById` | Returns full product details |
| `POST /SaveCartToUser` *(User)* | Persists cart to user profile |
| `POST /GetCart` *(User)* | Returns user’s saved cart |
| `POST /AddProductToCart` *(User)* | Adds item to cart |
| `POST /RemoveProductFromCart` *(User)* | Removes item from cart |
| `POST /CreateReview` *(User)* | Creates a new product review |
| `GET /GetUserReviews` *(User)* | Returns user’s reviews |
| `POST /CreateQuestion` *(User)* | Submits a product question |
| `GET /GetCategoryTree` | Full category tree |
| `GET /GetRootCategories` | Top-level categories |
| `GET /GetSubcategories` | Child categories |
| `POST /CreateOrder` *(User)* | Places a new order |
| `GET /GetOrders` *(User)* | Returns user order history |
| `GET /GetOrderById` *(User)* | Single order details |

---

### ✅ ShopController (shop owner functionality)

| Endpoint | Purpose |
|---------|---------|
| `POST /CreateProduct` | Create a new product |
| `POST /UpdateProduct` | Update existing product |
| `DELETE /DeleteProductById` | Hard delete a product |
| `GET /GetShopProducts` | List all shop-owned products |
| `GET /GetShopProductCards` | Summary listing |
| `POST /EditProductActiveStatus` | Toggle product visibility |
| `POST /EditProductStatus` | Update product status |
| `POST /EditDeliveryStatus` | Update delivery status of an order item |
| `POST /AnswerQuestion` | Reply to a user question |
| `GET /GetProductQuestions` | Retrieve questions |
| `GET /GetProductReviews` | Retrieve reviews |
| `GET /GetOrders` | All shop orders |
| `GET /GetOrdersByStatus` | Orders filtered by status |
| `POST /UpdateOrderStatus` | Change order state |

---

### ✅ AdminController (moderation & management)

| Endpoint | Purpose |
|---------|---------|
| `GET /GetAllProducts` | Admin product overview |
| `POST /EditProductApprovedStatus` | Approve / reject product |
| `POST /CreateCategory` *(SuperAdmin)* | Create new category |
| `GET /GetCategoryTree` | Get category structure |
| `DELETE /DeleteCategory` *(SuperAdmin)* | Delete category |
| `POST /UpdateCategory` *(SuperAdmin)* | Update category |
| `POST /EditReviewApprovedStatus` | Approve/reject reviews |

---

### ✅ PaymentsController

| Endpoint | Purpose |
|---------|---------|
| `POST /create-payment-intent` | Creates Stripe PaymentIntent (returns client secret) |

> Payment status is externally confirmed via Stripe.  
> The CoreAPI continues the internal **order lifecycle** afterward.

---

## 🚚 Order Status Lifecycle

| Status | Meaning |
|--------|---------|
| `New` | Order has been created |
| `Accepted` | Shop has confirmed and is processing |
| `Payed` | Stripe confirmed the payment |
| `Done` | Fulfilled / Delivered |
| `Canceled` | Order was canceled |

---

## 🧩 Error Handling & Response

The API uses a unified format: `ServiceResponse<T>`

| Field | Description |
|-------|-------------|
| `IsSuccess` | Operation result |
| `Entity/Entities` | Returned data |
| `Message` | Error or informational message |

Errors are created inside the **business layer**, ensuring clean controllers.

---

## ✉️ Email Notifications

- Users receive **order confirmation emails** after successful order placement.

---

## ☁️ Deployment & Cloud Readiness

| Feature | Description |
|---------|-------------|
| Independent deploy | Fully separated from AuthAPI |
| Own database | Business logic DB (not shared) |
| JWT protected | Only valid tokens accepted |
| Cloud ready | Suitable for Azure/App Service |

---

## ✅ Conclusion

The **MarketplaceCoreAPI** forms the **heart of the marketplace**, providing the central commerce logic: **browsing**, **buying**, and **managing** marketplace resources.  
Combined with the `MarketplaceAuthAPI`, it delivers a **modular, secure, and scalable** foundation for future marketplace extensions.

---
