# OrderManagement API

## Overview

This project is a RESTful API built using **ASP.NET Core 8 Web API** and **Entity Framework Core** to manage customer orders. It features customer-specific discount logic, status tracking, analytics, and performance optimizations.

---

## Key Features

- ✅ **Customer Segment-Based Discounting**  
  Discounts are applied based on customer segment and order history using the `DiscountService`.

- 📦 **Order Management**  
  - Create new orders with automatic discount application.  
  - Update the status of existing orders with valid transitions.  

- 📊 **Analytics Endpoint**  
  - Provides insights on average order value and average fulfillment time (from order creation to delivery).

---

## Performance Optimizations

- ⚡ **Caching**  
  `IMemoryCache` is used on the analytics endpoint to reduce database load. Cached results are stored for 1 minute.

- 🔄 **Asynchronous Programming**  
  All database operations are `async` to improve performance under load and ensure scalability.

---
##Assumptions
-Customer segments and discount logic are hardcoded for simplicity.
-Database is pre-seeded or connected to a persistent store for testing.
-Status enum controls valid order transitions.
-OrderService.UpdateStatus enforces transition rules.
---
##API Endpoints
Method	Endpoint	Description
POST	/api/orders/create	Creates an order with discount
PATCH		/api/orders/{id}/status	Updates order status
GET	/api/orders/analytics	order analytics with caching
---
Tools & Frameworks
-ASP.NET Core 8
-Entity Framework Core
-xUnit
-Swagger / Swashbuckle
-IMemoryCache
---
## Testing

### ✅ Unit Tests
Unit tests are written using **xUnit** and cover:
- Discount logic in `DiscountService`
- Order status update validation in `OrderService`

### 🔁 Integration Tests
Integration tests are written for controller endpoints using:
- `WebApplicationFactory<T>` to create a test server
- In-memory test client (`HttpClient`) to call real HTTP endpoints
- `AppDbContext` is used with test data to simulate real-world operations

Run tests using:
```bash
dotnet test
---



