# E-Commerce API - Agent Documentation

## Project Overview

**Project Name**: E-Commerce API (ASP.NET Core 8.0)  
**Purpose**: Production-ready e-commerce REST API with multi-role support (Admin, Vendor, Customer)

## Technology Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server with Entity Framework Core 8.0
- **Authentication**: JWT Bearer + ASP.NET Identity
- **Mapping**: AutoMapper 12.0
- **API Documentation**: Swagger (Swashbuckle)

## Project Structure

```
ECommerceAPI-ASP.NETCore/
├── Controllers/          # API endpoints
├── Data/                # DbContext
├── Migrations/          # EF Core migrations
├── Models/
│   ├── Domain/          # Entity classes
│   └── DTO/             # Data Transfer Objects
├── Repositories/
│   ├── Interface/       # Repository interfaces
│   └── Implementation/  # Repository implementations
└── Mappings/            # AutoMapper profiles
```

## Database Entities

### Core Entities
- **Product** - SKU, BasePrice, SalePrice, IsActive, Brand, Weight
- **Category** - Hierarchical with ParentCategory, DisplayOrder, IsActive
- **Stock** - SKU, Quantity, Price, LowStockThreshold, ReservedQuantity
- **Image** - FileName, Url, AltText, IsPrimary, DisplayOrder

### Order & Cart
- **Order** - OrderStatus enum (Pending → Cancelled), TotalAmount, Notes
- **OrderItem** - Quantity, UnitPrice, TotalPrice
- **ShoppingCart** - One-to-one with Customer
- **ShoppingCartItem** - Composite unique (CartId + StockId)
- **Rating** - Stars (1-5), IsVerifiedPurchase, unique (CustomerId + ProductId)

### New Entities (Phase 3)
- **Address** - Street, City, State, PostalCode, Country, AddressType
- **Payment** - PaymentMethod, PaymentStatus, TransactionId
- **Shipping** - Carrier, TrackingNumber, ShippingStatus
- **AuditLog** - Entity tracking, UserId, IpAddress, UserAgent

### Base Entity
All entities inherit from `BaseEntity`:
- Id (Guid)
- CreatedAt (DateTime)
- UpdatedAt (DateTime?)
- IsDeleted (bool) - for soft deletes

## Enums

| Enum | Values |
|------|--------|
| OrderStatus | Pending, Paid, Processing, Shipped, Delivered, Cancelled |
| PaymentMethod | CreditCard, DebitCard, PayPal, BankTransfer, CashOnDelivery, ApplePay, GooglePay |
| PaymentStatus | Pending, Processing, Completed, Failed, Refunded, Cancelled |
| ShippingStatus | Pending, LabelCreated, InTransit, OutForDelivery, Delivered, Failed, Returned |
| AddressType | Shipping, Billing, Both |

## Database Indexes

- Product: CategoryId, VendorId, SKU (unique), IsActive, BasePrice
- Stock: ProductId, SKU (unique)
- Category: ParentCategoryId
- ShoppingCart: CustomerId (unique)
- ShoppingCartItem: (ShoppingCartId, StockId) composite unique
- Order: CustomerId, Status
- OrderItem: OrderId
- Rating: (CustomerId, ProductId) unique, ProductId
- Image: Url
- Address: UserId, (UserId + IsDefault) filtered
- Payment: OrderId (unique), Status, TransactionId
- Shipping: OrderId (unique), Status, TrackingNumber
- AuditLog: EntityType, EntityId, Action, CreatedAt

## Key Configuration (Program.cs)

```csharp
// Database
builder.Services.AddDbContext<EcommerceDBContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("EcommerceConnectionString")));

// Identity
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<EcommerceDBContext>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });
```

## Running the Project

1. Ensure SQL Server is running
2. Update `appsettings.json` with connection string
3. Run migrations: `dotnet ef database update`
4. Run: `dotnet run`

## Testing Commands

```bash
# Build
dotnet build

# Run migrations
dotnet ef migrations add <MigrationName>
dotnet ef database update

# Run tests (if configured)
dotnet test
```

## Important Notes

- All domain entities inherit from `BaseEntity` for consistent audit fields
- Soft delete is supported via `IsDeleted` flag
- Product pricing uses BasePrice + optional SalePrice
- Order total is computed from OrderItems (not stored directly)
- ShoppingCart is unique per customer
- Rating allows one review per customer per product

## Known Warnings

- AutoMapper 12.0.0 has a known vulnerability (NU1903) - consider upgrading
- DTOs have nullable warnings that don't affect functionality

## Future Enhancements

- Add more validation on DTOs
- Implement soft delete in repositories
- Add audit logging middleware