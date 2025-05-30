# OrderManagement API
![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)
![Build](https://img.shields.io/badge/build-passing-brightgreen)
![Tests](https://img.shields.io/badge/tests-passing-green)
## Overview
This project is a RESTful API built using **ASP.NET Core 8 Web API** and **Entity Framework Core** to manage customer orders. It features customer-specific discount logic, status tracking, analytics, and performance optimizations.
---
## Key Features
- ✅ **Customer Segment-Based Discounting**  
  Discounts are applied based on customer segment and order history using the `DiscountService`.
- 📦 **Order Management**  
  - Create new orders with automatic discount application  
  - Update the status of existing orders with valid transitions  
- 📊 **Analytics Endpoint**  
  Provides insights on:
  - Average order value  
  - Average fulfillment time (from order creation to delivery)
---
## Performance Optimizations
- ⚡ **Caching**  
  `IMemoryCache` is used in the analytics endpoint to reduce database load. Cached results are stored for 1 minute.
- 🔄 **Asynchronous Programming**  
  All database operations are asynchronous (`async`) to improve performance under load and ensure scalability.
---
## Assumptions
- Customer segments and discount logic are hardcoded for simplicity  
- The database is pre-seeded or connected to a persistent store for testing  
- An `OrderStatus` enum controls valid order transitions  
- `OrderService.UpdateStatus()` enforces transition rules
---
## 📂 Database Script: `DbScript.sql`
This SQL script sets up the `OrderManagementDB` database schema and seeds initial data for testing and development.
### ✅ What It Includes
**Tables:**
- `Customers`: Stores customer details and segments  
- `Orders`: Stores customer orders with status and timestamps  
**Sample Data:**
- 3 predefined customers (`Regular`, `Premium`, `VIP`)  
- Multiple historical orders (including >5 for one customer to test discount logic and analytics)
### 🔧 Usage
1. Open SQL Server Management Studio (SSMS) or your preferred SQL tool  
2. Run the script to create and populate the `OrderManagementDB` database 
### 📝 Notes
- Timestamps use `GETDATE()` with offsets to simulate real-world order histories  
- Ensure your ASP.NET Core app’s connection string points to this database for full integration  
---
## API Endpoints
| Method | Endpoint                      | Description                        |
|--------|-------------------------------|------------------------------------|
| POST   | `/api/orders/create`          | Creates a new order with discount  |
| PATCH  | `/api/orders/{id}/status`     | Updates the order status           |
| GET    | `/api/orders/analytics`       | Retrieves cached order analytics   |
---
## Tools & Frameworks
- ASP.NET Core 8  
- Entity Framework Core  
- xUnit  
- Swagger / Swashbuckle  
- IMemoryCache  
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
- `AppDbContext` with test data to simulate real-world operations
### ▶️ Running Tests
```bash
dotnet test
