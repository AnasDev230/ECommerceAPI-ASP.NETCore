# ECommerceAPI Agent Guide

## Project Overview

**E-Commerce REST API** built with ASP.NET Core 8.0 Web API. Provides full CRUD operations for products, categories, shopping carts, orders, ratings, stocks, and image uploads. Uses SQL Server with Entity Framework Core, ASP.NET Core Identity for authentication, and JWT Bearer tokens for authorization.

### Framework & Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.NET.Sdk.Web | net8.0 | Web API framework |
| Microsoft.EntityFrameworkCore.SqlServer | 8.0.19 | SQL Server data access |
| Microsoft.EntityFrameworkCore.Tools | 8.0.19 | EF Core CLI tools (migrations) |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 8.0.19 | Identity + EF Core integration |
| Microsoft.AspNetCore.Authentication.JwtBearer | 8.0.19 | JWT authentication |
| Microsoft.IdentityModel.Tokens | 8.14.0 | Token validation |
| System.IdentityModel.Tokens.Jwt | 8.14.0 | JWT token creation |
| AutoMapper | 12.0.0 | Object-to-object mapping |
| AutoMapper.Extensions.Microsoft.DependencyInjection | 12.0.0 | AutoMapper DI integration |
| Swashbuckle.AspNetCore | 6.4.0 | Swagger/OpenAPI documentation |

### Key Characteristics

- **Target Framework:** .NET 8.0
- **Nullable Reference Types:** Enabled
- **Implicit Usings:** Enabled
- **Root Namespace:** `ECommerceAPI_ASP.NETCore`
- **Architecture:** Repository pattern with interface/implementation separation
- **Authentication:** ASP.NET Core Identity + JWT Bearer
- **Roles:** Admin, Vendor, Customer
- **Database:** SQL Server (EF Core 8.0.19, Code-First with migrations)
- **Object Mapping:** AutoMapper 12.0.0
- **API Docs:** Swagger UI at `/swagger`

---

## Build, Run, and Test Commands

### Prerequisites
- .NET 8.0 SDK
- SQL Server instance (connection string in `appsettings.json`)

### Commands

```bash
# Navigate to project directory
cd ECommerceAPI-ASP.NETCore/ECommerceAPI-ASP.NETCore

# Restore NuGet packages
dotnet restore

# Build the project
dotnet build

# Run the project (launches with https profile by default)
dotnet run

# Run in development environment
dotnet run --launch-profile https

# Build in Release mode
dotnet build -c Release

# Publish for deployment
dotnet publish -c Release -o ./publish
```

### URLs (from launchSettings.json)
- **HTTPS:** `https://localhost:7038`
- **HTTP:** `http://localhost:5127`
- **Swagger UI:** `https://localhost:7038/swagger`
- **IIS Express:** `https://localhost:44338`

### Testing
No test project currently exists. To add one:
```bash
dotnet new xunit -n ECommerceAPI.Tests
dotnet add ECommerceAPI-ASP.NETCore/ECommerceAPI-ASP.NETCore.csproj reference ECommerceAPI.Tests
```

---

## Folder Structure

```
ECommerceAPI-ASP.NETCore/
├── .gitignore
├── README.md
└── ECommerceAPI-ASP.NETCore/
    ├── ECommerceAPI-ASP.NETCore.sln
    └── ECommerceAPI-ASP.NETCore/
        ├── ECommerceAPI-ASP.NETCore.csproj
        ├── Program.cs
        ├── appsettings.json
        ├── appsettings.Development.json
        ├── ECommerceAPI-ASP.NETCore.http
        ├── Images/                              # Uploaded image files
        ├── Properties/
        │   └── launchSettings.json
        ├── Controllers/                         # API Controllers (9)
        │   ├── AuthController.cs
        │   ├── CategoriesController.cs
        │   ├── ImagesController.cs
        │   ├── OrdersController.cs
        │   ├── ProductsController.cs
        │   ├── RatingsController.cs
        │   ├── ShoppingCartItemsController.cs
        │   ├── ShoppingCartsController.cs
        │   └── StocksController.cs
        ├── Data/
        │   └── EcommerceDBContext.cs            # EF Core DbContext
        ├── Mappings/
        │   └── AutoMapperProfiles.cs            # AutoMapper configuration
        ├── Migrations/                          # EF Core migrations
        │   ├── 20250907160703_firstMigration.cs
        │   ├── 20250907160703_firstMigration.Designer.cs
        │   └── EcommerceDBContextModelSnapshot.cs
        ├── Models/
        │   ├── Domain/                          # Entity classes (15)
        │   │   ├── Address.cs
        │   │   ├── AuditLog.cs
        │   │   ├── BaseEntity.cs
        │   │   ├── Category.cs
        │   │   ├── Image.cs
        │   │   ├── Order.cs
        │   │   ├── OrderItem.cs
        │   │   ├── Payment.cs
        │   │   ├── Product.cs
        │   │   ├── Rating.cs
        │   │   ├── Shipping.cs
        │   │   ├── ShoppingCart.cs
        │   │   ├── ShoppingCartItem.cs
        │   │   ├── Stock.cs
        │   │   └── Transaction.cs
        │   └── DTO/                             # Request/Response DTOs (26)
        │       ├── Auth/
        │       │   ├── LoginRequestDto.cs
        │       │   ├── LoginResponseDto.cs
        │       │   ├── RegisterRequestDto.cs
        │       │   └── UpdatePasswordRequestDto.cs
        │       ├── Category/
        │       │   ├── CategoryDto.cs
        │       │   └── CreateCategoryRequestDto.cs
        │       ├── Image/
        │       │   └── ImageDto.cs
        │       ├── Order/
        │       │   ├── OrderDto.cs
        │       │   ├── CreateOrderRequestDto.cs
        │       │   ├── UpdateOrderStatusRequestDto.cs
        │       │   └── OrderItem/
        │       │       ├── OrderItemDto.cs
        │       │       └── CreateOrderItemRequestDto.cs
        │       ├── Product/
        │       │   ├── ProductDto.cs
        │       │   ├── CreateProductRequestDto.cs
        │       │   ├── Rating/
        │       │   │   ├── RatingDto.cs
        │       │   │   ├── CreateRatingRequestDto.cs
        │       │   │   ├── UpdateRatingRequestDto.cs
        │       │   │   └── GetMyRatingRequestDto.cs
        │       │   └── Stock/
        │       │       ├── StockDto.cs
        │       │       ├── CreateStockRequestDto.cs
        │       │       └── UpdateStockRequestDto.cs
        │       └── ShoppingCart/
        │           ├── ShoppingCartDto.cs
        │           ├── CreateShoppingCartRequestDto.cs
        │           └── ShoppingCartItem/
        │               ├── ShoppingCartItemDto.cs
        │               ├── CreateShoppingCartItemRequestDto.cs
        │               └── UpdateShoppingCartItemRequestDto.cs
        └── Repositories/
            ├── Interface/                       # Repository interfaces (13)
            │   ├── IAddressRepository.cs
            │   ├── IAuditLogRepository.cs
            │   ├── ICategoryRepository.cs
            │   ├── IImageRepository.cs
            │   ├── IOrderRepository.cs
            │   ├── IPaymentRepository.cs
            │   ├── IProductRepository.cs
            │   ├── IRatingRepository.cs
            │   ├── IShippingRepository.cs
            │   ├── IShoppingCartItemRepository.cs
            │   ├── IShoppingCartRepository.cs
            │   ├── IStockRepository.cs
            │   └── ITokenRepository.cs
            └── Implementation/                  # Repository implementations (13)
                ├── AddressRepository.cs
                ├── AuditLogRepository.cs
                ├── CategoryRepository.cs
                ├── ImageRepository.cs
                ├── OrderRepository.cs
                ├── PaymentRepository.cs
                ├── ProductRepository.cs
                ├── RatingRepository.cs
                ├── ShippingRepository.cs
                ├── ShoppingCartItemRepository.cs
                ├── ShoppingCartRepository.cs
                ├── StockRepository.cs
                └── TokenRepository.cs
```

---

## Code Conventions

### General
- **Namespace:** `ECommerceAPI_ASP.NETCore` (note: dots replace hyphens)
- **Nullable reference types:** Enabled — use `?` for nullable types
- **Implicit usings:** Enabled — common namespaces auto-imported
- **Brace style:** Allman (opening braces on new line)
- **Field naming:** `camelCase` with `this.` prefix for constructor assignment
- **No comments** unless explicitly requested

### Domain Models (`Models/Domain/`)
- All entities inherit from `BaseEntity`
- `BaseEntity` provides: `Id` (Guid), `CreatedAt` (DateTime.UtcNow), `UpdatedAt?` (DateTime?), `IsDeleted` (bool)
- Use `[Required]`, `[MaxLength(n)]`, `[Range(min, max)]` data annotations
- Navigation properties are nullable reference types (`?`)
- Collection navigation properties initialized to empty collections: `= new List<T>()`
- String properties default to `string.Empty`

```csharp
public class Product : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public Guid? ImageID { get; set; }
    public Image? Image { get; set; }

    [Required]
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
```

### DTOs (`Models/DTO/`)
- Organized in subfolders by domain (Auth, Category, Product, Order, ShoppingCart, Image)
- Nested DTOs in sub-subfolders (e.g., `Product/Rating/`, `Product/Stock/`, `Order/OrderItem/`)
- Naming convention: `<Entity>Dto`, `Create<Entity>RequestDto`, `Update<Entity>RequestDto`
- Use same data annotations as domain models for validation
- Do NOT include navigation properties or audit fields (CreatedAt, UpdatedAt) unless needed for response

### Controllers (`Controllers/`)
- Inherit from `ControllerBase` with `[ApiController]` attribute
- Route template: `[Route("api/[controller]")]`
- Constructor injection: `private readonly IRepository repository;` with `this.repository = repository;`
- Action methods are `public async Task<IActionResult>`
- Named routes: `[HttpPost("ActionName", Name = "ActionName")]`
- Explicit response types: `[ProducesResponseType(StatusCodes.Status200OK)]`
- Role-based authorization: `[Authorize(Roles = "Admin,Vendor")]`
- Get current user ID: `User.FindFirstValue(ClaimTypes.NameIdentifier)`
- Return patterns:
  - Create: `return Created("", mapper.Map<EntityDto>(entity));`
  - Read: `return Ok(mapper.Map<EntityDto>(entity));`
  - Not found: `return NotFound();`
  - Unauthorized: `return Unauthorized();`
  - Bad request: `return BadRequest("Message");`

```csharp
[HttpPost("Add", Name = "AddProduct")]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[Authorize(Roles = "Admin,Vendor")]
public async Task<IActionResult> AddProduct([FromBody] CreateProductRequestDto request)
{
    var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (vendorId == null)
        return Unauthorized();

    var product = new Product
    {
        Name = request.Name,
        Description = request.Description,
        ImageID = request.ImageID,
        VendorId = vendorId,
        CategoryId = request.CategoryId,
    };

    await productRepository.CreateAsync(product);
    return Created("", mapper.Map<ProductDto>(product));
}
```

### Repositories

**Interface pattern** (`Repositories/Interface/`):
```csharp
public interface IProductRepository
{
    Task<Product> CreateAsync(Product product);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByID(Guid id);
    Task<bool> UpdateAsync(Product product);
    Task<Product?> DeleteAsync(Guid id);
}
```

**Implementation pattern** (`Repositories/Implementation/`):
- Inject `EcommerceDBContext` via constructor
- Use `AsNoTracking()` for read queries
- Use `Include()` for eager loading navigation properties
- Use `AsSplitQuery()` for queries with multiple collection includes
- Use `ExecuteUpdateAsync`/`ExecuteDeleteAsync` for bulk operations
- Use `IDbContextTransaction` for operations requiring transactions

```csharp
public class ProductRepository : IProductRepository
{
    private readonly EcommerceDBContext dbContext;

    public ProductRepository(EcommerceDBContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Product?> GetByID(Guid id)
    {
        return await dbContext.Products
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Category)
            .Include(p => p.Image)
            .Include(p => p.Stocks)
            .Include(p => p.Ratings)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
```

### AutoMapper (`Mappings/AutoMapperProfiles.cs`)
- Single profile class inheriting from `Profile`
- Bidirectional maps: `CreateMap<Entity, EntityDto>().ReverseMap();`
- Create-only maps: `CreateMap<CreateEntityRequestDto, Entity>();`
- Update maps (for `mapper.Map(source, destination)`): `CreateMap<UpdateEntityRequestDto, Entity>();`
- Register in `Program.cs`: `builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));`

### DbContext (`Data/EcommerceDBContext.cs`)
- Inherits from `IdentityDbContext<IdentityUser>`
- DbSet properties for each entity
- `OnModelCreating` configures:
  - Relationships with `.HasOne().WithMany().HasForeignKey()`
  - Delete behaviors: `DeleteBehavior.Restrict`, `DeleteBehavior.Cascade`, `DeleteBehavior.SetNull`
  - Indexes: `.HasIndex().IsUnique().HasFilter()` for filtered unique indexes

---

## Database Commands (EF Core)

Run from the project directory containing `.csproj`:

```bash
# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations to database
dotnet ef database update

# Apply a specific migration
dotnet ef database update MigrationName

# Remove the last migration (if not applied)
dotnet ef migrations remove

# List all migrations
dotnet ef migrations list

# Drop the database
dotnet ef database drop

# Scaffold DbContext from existing database
dotnet ef dbcontext scaffold "Server=...;Database=...;Trusted_Connection=true;" Microsoft.EntityFrameworkCore.SqlServer -o Models/Domain -c EcommerceDBContext --force
```

### Current Connection String
```
Server=LAPTOP-VNSR0OVO;Database=EcommerceDB;Trusted_Connection=true;TrustServerCertificate=True
```

---

## Environment Variables & Configuration

### appsettings.json Keys

| Key | Description |
|-----|-------------|
| `ConnectionStrings:EcommerceConnectionString` | SQL Server connection string |
| `Jwt:Key` | HMAC-SHA256 signing key for JWT tokens |
| `Jwt:Issuer` | JWT token issuer (default: `https://localhost:7038/`) |
| `Jwt:Audience` | JWT token audience (default: `https://localhost:7038/`) |

### Environment Variables

| Variable | Description |
|----------|-------------|
| `ASPNETCORE_ENVIRONMENT` | `Development`, `Staging`, or `Production` |

### Secrets (for local development, replace appsettings values)

```bash
# Set user secrets
dotnet user-secrets set "Jwt:Key" "your-32-char-minimum-secret-key-here"
dotnet user-secrets set "ConnectionStrings:EcommerceConnectionString" "Server=...;Database=...;Trusted_Connection=true;TrustServerCertificate=True"

# List secrets
dotnet user-secrets list

# Remove a secret
dotnet user-secrets remove "Jwt:Key"

# Clear all secrets
dotnet user-secrets clear
```

### Identity Password Policy
- Minimum length: 6
- No digit required
- No lowercase required
- No uppercase required
- No special character required
- Minimum unique characters: 1

### JWT Token Settings
- Token lifetime: 60 minutes
- Validation: Issuer, Audience, Lifetime, SigningKey all validated

---

## API Endpoints Summary

### Auth (`/api/Auth`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/api/Auth/Register` | None | Register new user |
| POST | `/api/Auth/Login` | None | Login and get JWT token |
| DELETE | `/api/Auth/DeleteAccount` | Admin,Vendor,Customer | Delete own account |
| PUT | `/api/Auth/ChangePassword` | Admin,Vendor,Customer | Change password |

### Categories (`/api/Categories`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/api/Categories/Add` | Admin | Create category |
| GET | `/api/Categories` | None | List all categories |
| GET | `/api/Categories/GetByID/{id}` | None | Get category by ID |
| PUT | `/api/Categories/{id}` | Admin | Update category |
| DELETE | `/api/Categories/{id}` | Admin | Delete category |

### Products (`/api/Products`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/api/Products/Add` | Admin,Vendor | Create product |
| GET | `/api/Products/GetByID/{id}` | Admin,Vendor,Customer | Get product by ID |
| GET | `/api/Products/GetProductsByCategoryID/{id}` | Admin,Vendor,Customer | Get products by category |
| PUT | `/api/Products/{id}` | Admin,Vendor | Update product |
| DELETE | `/api/Products/{id}` | Admin,Vendor | Delete product |

### Stocks (`/api/Stocks`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/api/Stocks/Add` | Admin,Vendor | Create stock variant |
| GET | `/api/Stocks/{productID}` | None | Get stocks by product |
| GET | `/api/Stocks/GetByID/{stockID}` | None | Get stock by ID |
| PUT | `/api/Stocks/{stockID}` | Admin,Vendor | Update stock |
| DELETE | `/api/Stocks/{stockID}` | Admin,Vendor | Delete stock |

### Shopping Carts (`/api/ShoppingCarts`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/api/ShoppingCarts/Add` | Customer | Create cart |
| GET | `/api/ShoppingCarts/GetByCustomerID` | Customer | Get own cart |
| GET | `/api/ShoppingCarts` | None | Get cart by ID |
| DELETE | `/api/ShoppingCarts` | Customer | Delete own cart |

### Cart Items (`/api/ShoppingCartItems`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/api/ShoppingCartItems/Add` | Customer | Add item to cart |
| GET | `/api/ShoppingCartItems` | None | Get item by ID |
| GET | `/api/ShoppingCartItems/{id}` | None | Get items by cart ID |
| PUT | `/api/ShoppingCartItems` | Customer | Update item quantity |
| DELETE | `/api/ShoppingCartItems` | Customer | Remove item from cart |

### Ratings (`/api/Ratings`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/api/Ratings/Add` | Admin,Vendor,Customer | Add rating |
| GET | `/api/Ratings` | Admin,Vendor,Customer | Get own rating for product |
| GET | `/api/Ratings/{productID}` | None | Get all ratings for product |
| PUT | `/api/Ratings/{productId}` | Admin,Vendor,Customer | Update own rating |
| DELETE | `/api/Ratings/{productId}` | Admin,Vendor,Customer | Delete own rating |

### Orders (`/api/Orders`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/api/Orders` | Customer | Create order from cart |
| GET | `/api/Orders/{orderID}` | Customer | Get order by ID |
| GET | `/api/Orders` | Admin | List all orders |
| PUT | `/api/Orders/{orderId}` | Admin | Update order status |
| DELETE | `/api/Orders/{orderId}` | Customer | Delete order |

### Images (`/api/Images`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/api/Images` | Admin,Vendor,Customer | Upload image file |
| GET | `/api/Images` | Admin | List all images |
| GET | `/api/Images/{id}` | None | Get image by ID |
| DELETE | `/api/Images/{id}` | None | Delete image |

### Repositories Without Controllers (not yet exposed via API)
- **Address** — `IAddressRepository` (CRUD + SetDefault)
- **Payment** — `IPaymentRepository` (CRUD + UpdateStatus)
- **Shipping** — `IShippingRepository` (CRUD + UpdateStatus)
- **AuditLog** — `IAuditLogRepository` (CRUD + Query by entity/user/action/date)

---

## Domain Entities

### BaseEntity (abstract)
| Property | Type | Description |
|----------|------|-------------|
| Id | Guid | Primary key |
| CreatedAt | DateTime | Auto-set to UtcNow |
| UpdatedAt | DateTime? | Nullable, set on update |
| IsDeleted | bool | Soft delete flag |

### Key Entities
| Entity | Key Fields | Relationships |
|--------|-----------|---------------|
| **Category** | Name, Description, DisplayOrder, IsActive, ImageID, ParentCategoryId | Self-referencing (ParentCategory/SubCategories), has Products, has Image |
| **Product** | Name, Description, Brand, IsActive, Weight, ImageID, VendorId, CategoryId | belongs to Category, has Image, has Stocks, has Ratings, belongs to Vendor (IdentityUser) |
| **Stock** | ProductId, SKU, ImageID, Color, Size, Quantity, Price, LowStockThreshold, ReservedQuantity | belongs to Product, has Image |
| **ShoppingCart** | CustomerId | belongs to Customer (IdentityUser), has Items |
| **ShoppingCartItem** | ShoppingCartId, StockId, Quantity | belongs to ShoppingCart, belongs to Stock |
| **Rating** | ProductId, CustomerId, Stars (1-5), Comment, IsVerifiedPurchase | belongs to Product, belongs to Customer |
| **Order** | CustomerId, CompletedAt, TotalAmount, Status, Notes, BillingAddressId | belongs to Customer, has Items, has Payment, has Shipping, has BillingAddress |
| **OrderItem** | OrderId, StockId, ProductNameSnapshot, VariantDetailsSnapshot, Quantity, UnitPrice, TotalPrice, Notes | belongs to Order, belongs to Stock |
| **Image** | FileName, FileExtension, Title, AltText, Url, IsPrimary, DisplayOrder | Referenced by Product, Category, Stock |
| **Address** | UserId, Street, City, State, PostalCode, Country, PhoneNumber, IsDefault, AddressType | belongs to User (IdentityUser) |
| **Payment** | OrderId, Method, Amount, Status, ProcessedAt, FailureReason | belongs to Order (1:1), has Transactions |
| **Shipping** | OrderId, Carrier, TrackingNumber, EstimatedDelivery, ActualDelivery, Status, Notes, ShippingAddressId | belongs to Order (1:1), has ShippingAddress |
| **AuditLog** | EntityType, EntityId, Action, OldValues, NewValues, UserId, Description, IpAddress, UserAgent | belongs to User |
| **Transaction** | PaymentId, GatewayTransactionId, Amount, Type, Status, GatewayRawResponse, ErrorCode, ErrorDescription | belongs to Payment |

### Enums
| Enum | Values |
|------|--------|
| **OrderStatus** | Pending, Paid, Processing, Shipped, Delivered, Cancelled |
| **AddressType** | Shipping, Billing, Both |
| **PaymentMethod** | CreditCard, DebitCard, PayPal, BankTransfer, CashOnDelivery, ApplePay, GooglePay |
| **PaymentStatus** | Pending, Processing, Completed, Failed, Refunded, Cancelled |
| **ShippingStatus** | Pending, LabelCreated, InTransit, OutForDelivery, Delivered, Failed, Returned |
| **TransactionType** | Authorize, Capture, Refund, Void, Chargeback |
| **TransactionStatus** | Pending, Success, Failed |

---

## Common Tasks for AI Agents

### How to Add a New Controller

1. **Create the DTOs** in `Models/DTO/<EntityName>/`:
   - `<EntityName>Dto.cs` (response)
   - `Create<EntityName>RequestDto.cs` (create request)
   - `Update<EntityName>RequestDto.cs` (update request, optional)

2. **Create the domain model** in `Models/Domain/<EntityName>.cs`:
   - Inherit from `BaseEntity`
   - Add data annotations (`[Required]`, `[MaxLength]`, `[Range]`)
   - Add navigation properties

3. **Add DbSet to DbContext** in `Data/EcommerceDBContext.cs`:
   ```csharp
   public DbSet<EntityName> EntityNames { get; set; }
   ```

4. **Configure relationships** in `OnModelCreating` in `EcommerceDBContext.cs`

5. **Create the repository interface** in `Repositories/Interface/I<EntityName>Repository.cs`

6. **Create the repository implementation** in `Repositories/Implementation/<EntityName>Repository.cs`

7. **Register the repository** in `Program.cs`:
   ```csharp
   builder.Services.AddScoped<I<EntityName>Repository, <EntityName>Repository>();
   ```

8. **Add AutoMapper mappings** in `Mappings/AutoMapperProfiles.cs`:
   ```csharp
   CreateMap<EntityName, EntityNameDto>().ReverseMap();
   CreateMap<CreateEntityNameRequestDto, EntityName>();
   CreateMap<UpdateEntityNameRequestDto, EntityName>(); // if applicable
   ```

9. **Create the controller** in `Controllers/<EntityName>Controller.cs`:
   - Follow the existing controller conventions (see Code Conventions section)
   - Inject repository and IMapper via constructor
   - Add `[ProducesResponseType]` attributes
   - Add `[Authorize(Roles = "...")]` where appropriate

10. **Create and apply migration**:
    ```bash
    dotnet ef migrations add Add<EntityName>
    dotnet ef database update
    ```

### How to Add a New Migration

```bash
# From the project directory (where .csproj is)
dotnet ef migrations add DescriptionOfChange
dotnet ef database update
```

### How to Add a New Repository

1. Create interface in `Repositories/Interface/I<Name>Repository.cs`
2. Create implementation in `Repositories/Implementation/<Name>Repository.cs`
3. Register in `Program.cs`: `builder.Services.AddScoped<I<Name>Repository, <Name>Repository>();`

### How to Add a New DTO

1. Create file in appropriate subfolder under `Models/DTO/`
2. Add `using System.ComponentModel.DataAnnotations;` if using validation attributes
3. Match property names to domain model (for AutoMapper compatibility)
4. Add same validation attributes as domain model

### How to Add AutoMapper Mapping

1. Open `Mappings/AutoMapperProfiles.cs`
2. Add mapping in the constructor:
   ```csharp
   CreateMap<Source, Destination>().ReverseMap(); // bidirectional
   CreateMap<CreateRequestDto, Entity>();          // create only
   CreateMap<UpdateRequestDto, Entity>();          // update (maps onto existing entity)
   ```

### How to Add Role-Based Authorization

```csharp
[Authorize(Roles = "Admin")]           // Admin only
[Authorize(Roles = "Admin,Vendor")]    // Admin or Vendor
[Authorize(Roles = "Admin,Vendor,Customer")] // All authenticated roles
```

### How to Get Current User Info

```csharp
// Get user ID
var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

// Get user email
var email = User.FindFirstValue(ClaimTypes.Email);

// Get user roles
var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
```

### How to Handle File Uploads

Follow the pattern in `ImagesController.cs` and `ImageRepository.cs`:
- Accept `IFormFile` in controller
- Repository handles: extension validation, size validation (5MB max), GUID filename generation, physical file save to `Images/` folder, DB record creation
- Static files served from `/Images` path via `UseStaticFiles` in `Program.cs`

### How to Use Transactions

Follow the pattern in `ShoppingCartItemRepository.cs`:
```csharp
using var transaction = await dbContext.Database.BeginTransactionAsync();
try
{
    // ... operations
    await dbContext.SaveChangesAsync();
    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}
```

---

## Program.cs Service Registration Order

Services are registered in `Program.cs` in this order:
1. `AddControllers()` — MVC controllers
2. `AddHttpContextAccessor()` — HTTP context access
3. `AddEndpointsApiExplorer()` — API explorer for Swagger
4. `AddSwaggerGen()` — Swagger with JWT security definition
5. `AddDbContext<EcommerceDBContext>()` — EF Core with SQL Server
6. `AddScoped<...Repository, ...Repository>()` — All repositories (13 total)
7. `AddAutoMapper(typeof(AutoMapperProfiles))` — AutoMapper
8. `AddIdentityCore<IdentityUser>()` — Identity with custom password policy
9. `AddAuthentication(JwtBearerDefaults.AuthenticationScheme)` — JWT validation

Middleware pipeline order:
1. `UseSwagger()` / `UseSwaggerUI()` (Development only)
2. `UseHttpsRedirection()`
3. `UseAuthorization()`
4. `UseStaticFiles()` (Images folder)
5. `MapControllerRoute()` (default route)
6. `MapControllers()`
