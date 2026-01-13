# Oman Digital Shop - Backend API Schema

> Complete API documentation for Angular Frontend Integration

---

## Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [Base URL & Configuration](#base-url--configuration)
3. [Authentication](#authentication)
4. [API Endpoints](#api-endpoints)
   - [Auth API](#auth-api)
   - [Products API](#products-api)
   - [Categories API](#categories-api)
   - [Orders API](#orders-api)
5. [Data Models](#data-models)
6. [Error Handling](#error-handling)
7. [Angular Integration Guide](#angular-integration-guide)

---

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────────┐
│                     Angular Frontend                             │
│                   (http://localhost:4200)                        │
└─────────────────────────────────┬───────────────────────────────┘
                                  │ HTTP/HTTPS
                                  ▼
┌─────────────────────────────────────────────────────────────────┐
│                    REST API Layer                                │
│              Pll.Api.OmanDigitalShop                            │
│           (https://localhost:7144/api)                          │
│                                                                  │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐ ┌────────────┐ │
│  │   Auth      │ │  Products   │ │ Categories  │ │   Orders   │ │
│  │ Controller  │ │ Controller  │ │ Controller  │ │ Controller │ │
│  └─────────────┘ └─────────────┘ └─────────────┘ └────────────┘ │
└─────────────────────────────────┬───────────────────────────────┘
                                  │
                                  ▼
┌─────────────────────────────────────────────────────────────────┐
│                    Service Layer (SLL)                          │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐               │
│  │  Product    │ │  Category   │ │   Order     │               │
│  │  Service    │ │  Service    │ │  Service    │               │
│  └─────────────┘ └─────────────┘ └─────────────┘               │
└─────────────────────────────────┬───────────────────────────────┘
                                  │
                                  ▼
┌─────────────────────────────────────────────────────────────────┐
│                   Repository Layer (BLL)                        │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐               │
│  │  Product    │ │  Category   │ │   Order     │               │
│  │  Repository │ │  Repository │ │  Repository │               │
│  └─────────────┘ └─────────────┘ └─────────────┘               │
│                         │                                        │
│              ┌──────────┴──────────┐                            │
│              │  GenericRepository  │                            │
│              └──────────┬──────────┘                            │
└─────────────────────────┼───────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────────────┐
│                   Database (SQL Server)                         │
│                     OmanEcommerce                               │
└─────────────────────────────────────────────────────────────────┘
```

---

## Base URL & Configuration

### Development
```
API Base URL: https://localhost:7144/api
Swagger UI:   https://localhost:7144/swagger
```

### CORS Configuration
The API is configured to allow requests from:
- `http://localhost:4200` (Angular default)
- `http://localhost:3000`
- `https://localhost:4200`

### Headers Required
```http
Content-Type: application/json
Authorization: Bearer {jwt_token}  # For protected endpoints
```

---

## Authentication

### JWT Configuration
| Property | Value |
|----------|-------|
| Token Type | Bearer |
| Expiration | 7 days |
| Algorithm | HMAC SHA256 |

### Token Claims
```json
{
  "nameid": "user-id",
  "email": "user@email.com",
  "name": "Full Name",
  "role": ["Customer", "Admin"],
  "jti": "unique-token-id",
  "exp": 1234567890
}
```

---

## API Endpoints

### Auth API

#### Register User
```http
POST /api/auth/register
```

**Request Body:**
```json
{
  "firstName": "Ahmed",
  "lastName": "Al-Balushi",
  "email": "ahmed@example.com",
  "password": "Password123",
  "confirmPassword": "Password123"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "تم التسجيل بنجاح",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2024-01-08T10:00:00Z",
  "user": {
    "id": "abc123",
    "email": "ahmed@example.com",
    "firstName": "Ahmed",
    "lastName": "Al-Balushi",
    "fullName": "Ahmed Al-Balushi",
    "roles": ["Customer"]
  }
}
```

---

#### Login
```http
POST /api/auth/login
```

**Request Body:**
```json
{
  "email": "ahmed@example.com",
  "password": "Password123"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "تم تسجيل الدخول بنجاح",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2024-01-08T10:00:00Z",
  "user": {
    "id": "abc123",
    "email": "ahmed@example.com",
    "firstName": "Ahmed",
    "lastName": "Al-Balushi",
    "fullName": "Ahmed Al-Balushi",
    "roles": ["Customer"]
  }
}
```

---

#### Get Current User
```http
GET /api/auth/me
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "id": "abc123",
  "email": "ahmed@example.com",
  "firstName": "Ahmed",
  "lastName": "Al-Balushi",
  "fullName": "Ahmed Al-Balushi",
  "roles": ["Customer"]
}
```

---

### Products API

#### Get All Products
```http
GET /api/products
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "iPhone 15 Pro",
    "description": "Latest Apple smartphone",
    "price": 450.000,
    "stockQuantity": 50,
    "imageUrl": "/images/iphone15.jpg",
    "isActive": true,
    "createdAt": "2024-01-01T00:00:00",
    "categoryId": 1,
    "category": {
      "id": 1,
      "name": "Electronics"
    }
  }
]
```

---

#### Get Active Products Only
```http
GET /api/products/active
```

---

#### Get Product by ID
```http
GET /api/products/{id}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "name": "iPhone 15 Pro",
  "description": "Latest Apple smartphone",
  "price": 450.000,
  "stockQuantity": 50,
  "imageUrl": "/images/iphone15.jpg",
  "isActive": true,
  "createdAt": "2024-01-01T00:00:00",
  "categoryId": 1,
  "category": {
    "id": 1,
    "name": "Electronics"
  }
}
```

**Response (404 Not Found):**
```json
{
  "message": "المنتج غير موجود",
  "productId": 999
}
```

---

#### Get Products by Category
```http
GET /api/products/category/{categoryId}
```

---

#### Search Products
```http
GET /api/products/search?term={searchTerm}
```

**Example:** `GET /api/products/search?term=iPhone`

---

#### Create Product
```http
POST /api/products
```

**Request Body:**
```json
{
  "name": "Samsung Galaxy S24",
  "description": "Latest Samsung flagship",
  "price": 399.000,
  "stockQuantity": 100,
  "imageUrl": "/images/samsung-s24.jpg",
  "isActive": true,
  "categoryId": 1
}
```

**Response (201 Created):**
```json
{
  "id": 2,
  "name": "Samsung Galaxy S24",
  "description": "Latest Samsung flagship",
  "price": 399.000,
  "stockQuantity": 100,
  "imageUrl": "/images/samsung-s24.jpg",
  "isActive": true,
  "createdAt": "2024-01-01T10:00:00",
  "categoryId": 1
}
```

---

#### Update Product
```http
PUT /api/products/{id}
```

**Request Body:**
```json
{
  "id": 1,
  "name": "iPhone 15 Pro Max",
  "description": "Updated description",
  "price": 499.000,
  "stockQuantity": 45,
  "imageUrl": "/images/iphone15-pro-max.jpg",
  "isActive": true,
  "categoryId": 1
}
```

**Response:** `204 No Content`

---

#### Delete Product
```http
DELETE /api/products/{id}
```

**Response:** `204 No Content`

---

#### Get Products Count
```http
GET /api/products/count
```

**Response (200 OK):**
```json
{
  "count": 150
}
```

---

### Categories API

#### Get All Categories
```http
GET /api/categories
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Electronics",
    "description": "Electronic devices and gadgets",
    "imageUrl": "/images/electronics.jpg",
    "isActive": true
  },
  {
    "id": 2,
    "name": "Clothing",
    "description": "Fashion and apparel",
    "imageUrl": "/images/clothing.jpg",
    "isActive": true
  }
]
```

---

#### Get Active Categories
```http
GET /api/categories/active
```

---

#### Get Category by ID
```http
GET /api/categories/{id}
```

---

#### Get Category with Products
```http
GET /api/categories/{id}/products
```

**Response (200 OK):**
```json
{
  "id": 1,
  "name": "Electronics",
  "description": "Electronic devices and gadgets",
  "imageUrl": "/images/electronics.jpg",
  "isActive": true,
  "products": [
    {
      "id": 1,
      "name": "iPhone 15 Pro",
      "price": 450.000
    },
    {
      "id": 2,
      "name": "Samsung Galaxy S24",
      "price": 399.000
    }
  ]
}
```

---

#### Create Category
```http
POST /api/categories
```

**Request Body:**
```json
{
  "name": "Home Appliances",
  "description": "Home and kitchen appliances",
  "imageUrl": "/images/appliances.jpg",
  "isActive": true
}
```

**Response (201 Created):** Returns created category

---

#### Update Category
```http
PUT /api/categories/{id}
```

**Response:** `204 No Content`

---

#### Delete Category
```http
DELETE /api/categories/{id}
```

**Response:** `204 No Content`

---

#### Get Categories Count
```http
GET /api/categories/count
```

**Response (200 OK):**
```json
{
  "count": 10
}
```

---

### Orders API

#### Get All Orders
```http
GET /api/orders
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "orderDate": "2024-01-01T10:00:00",
    "status": 0,
    "shippingAddress": "123 Main St",
    "city": "Muscat",
    "postalCode": "100",
    "phoneNumber": "+968 9999 9999",
    "subTotal": 500.000,
    "discount": 50.000,
    "totalAmount": 450.000,
    "appUserId": "abc123",
    "appUser": {
      "firstName": "Ahmed",
      "lastName": "Al-Balushi"
    },
    "orderItems": [
      {
        "id": 1,
        "quantity": 1,
        "unitPrice": 450.000,
        "productId": 1,
        "product": {
          "name": "iPhone 15 Pro"
        }
      }
    ]
  }
]
```

---

#### Get Order by ID
```http
GET /api/orders/{id}
```

---

#### Get Orders by User
```http
GET /api/orders/user/{userId}
```

---

#### Get Orders by Status
```http
GET /api/orders/status/{status}
```

**Status Values:**
| Value | Status |
|-------|--------|
| 0 | Pending |
| 1 | Confirmed |
| 2 | Shipping |
| 3 | Delivered |
| 4 | Cancelled |

---

#### Get Pending Orders
```http
GET /api/orders/pending
```

---

#### Create Order
```http
POST /api/orders
```

**Request Body:**
```json
{
  "shippingAddress": "123 Main Street, Building A",
  "city": "Muscat",
  "postalCode": "100",
  "phoneNumber": "+968 9999 9999",
  "discount": 0,
  "appUserId": "user-id-from-token",
  "orderItems": [
    {
      "productId": 1,
      "quantity": 2,
      "unitPrice": 450.000
    },
    {
      "productId": 3,
      "quantity": 1,
      "unitPrice": 25.000
    }
  ]
}
```

**Response (201 Created):** Returns created order with calculated totals

---

#### Update Order Status
```http
PUT /api/orders/{id}/status
```

**Request Body:**
```json
{
  "status": 1
}
```

**Response (200 OK):**
```json
{
  "message": "تم تحديث حالة الطلب",
  "orderId": 1,
  "newStatus": "Confirmed"
}
```

---

#### Confirm Order
```http
PUT /api/orders/{id}/confirm
```

---

#### Ship Order
```http
PUT /api/orders/{id}/ship
```

---

#### Deliver Order
```http
PUT /api/orders/{id}/deliver
```

---

#### Cancel Order
```http
PUT /api/orders/{id}/cancel
```

---

#### Get Orders Count
```http
GET /api/orders/count
```

---

#### Get Pending Orders Count
```http
GET /api/orders/pending/count
```

---

## Data Models

### Product
```typescript
interface Product {
  id: number;
  name: string;
  description?: string;
  price: number;
  stockQuantity: number;
  imageUrl?: string;
  isActive: boolean;
  createdAt: Date;
  categoryId: number;
  category?: Category;
}
```

### Category
```typescript
interface Category {
  id: number;
  name: string;
  description?: string;
  imageUrl?: string;
  isActive: boolean;
  products?: Product[];
}
```

### Order
```typescript
interface Order {
  id: number;
  orderDate: Date;
  status: OrderStatus;
  shippingAddress: string;
  city: string;
  postalCode?: string;
  phoneNumber: string;
  subTotal: number;
  discount: number;
  totalAmount: number;
  appUserId: string;
  appUser?: AppUser;
  orderItems: OrderItem[];
}

enum OrderStatus {
  Pending = 0,
  Confirmed = 1,
  Shipping = 2,
  Delivered = 3,
  Cancelled = 4
}
```

### OrderItem
```typescript
interface OrderItem {
  id: number;
  quantity: number;
  unitPrice: number;
  orderId: number;
  productId: number;
  product?: Product;
  totalPrice: number; // Computed: quantity * unitPrice
}
```

### User (AppUser)
```typescript
interface AppUser {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  fullName: string;
  createdAt: Date;
  roles: string[];
}
```

### Auth DTOs
```typescript
interface LoginDto {
  email: string;
  password: string;
}

interface RegisterDto {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

interface AuthResponse {
  success: boolean;
  message: string;
  token?: string;
  expiration?: Date;
  user?: UserDto;
}
```

---

## Error Handling

### Error Response Format
```json
{
  "message": "Error description",
  "errors": {
    "fieldName": ["Validation error message"]
  }
}
```

### HTTP Status Codes

| Code | Description |
|------|-------------|
| 200 | Success |
| 201 | Created |
| 204 | No Content (Success with no body) |
| 400 | Bad Request (Validation errors) |
| 401 | Unauthorized (Missing/Invalid token) |
| 403 | Forbidden (Insufficient permissions) |
| 404 | Not Found |
| 409 | Conflict (Duplicate data) |
| 500 | Internal Server Error |

---

## Angular Integration Guide

### 1. Environment Configuration

**environment.ts:**
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7144/api'
};
```

### 2. Auth Service Example

```typescript
@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  login(credentials: LoginDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/login`, credentials);
  }

  register(data: RegisterDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/register`, data);
  }

  getCurrentUser(): Observable<UserDto> {
    return this.http.get<UserDto>(`${this.apiUrl}/auth/me`);
  }

  // Store token in localStorage
  setToken(token: string): void {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  logout(): void {
    localStorage.removeItem('token');
  }
}
```

### 3. HTTP Interceptor for JWT

```typescript
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getToken();

    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(req);
  }
}
```

### 4. Product Service Example

```typescript
@Injectable({ providedIn: 'root' })
export class ProductService {
  private apiUrl = `${environment.apiUrl}/products`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }

  getActive(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.apiUrl}/active`);
  }

  getById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`);
  }

  getByCategory(categoryId: number): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.apiUrl}/category/${categoryId}`);
  }

  search(term: string): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.apiUrl}/search`, {
      params: { term }
    });
  }

  create(product: Product): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, product);
  }

  update(id: number, product: Product): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, product);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
```

### 5. Order Service Example

```typescript
@Injectable({ providedIn: 'root' })
export class OrderService {
  private apiUrl = `${environment.apiUrl}/orders`;

  constructor(private http: HttpClient) {}

  getMyOrders(userId: string): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.apiUrl}/user/${userId}`);
  }

  createOrder(order: CreateOrderDto): Observable<Order> {
    return this.http.post<Order>(this.apiUrl, order);
  }

  cancelOrder(orderId: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/${orderId}/cancel`, {});
  }
}
```

---

## Quick Reference Card

### Endpoints Summary

| Method | Endpoint | Description |
|--------|----------|-------------|
| **Auth** |||
| POST | /api/auth/register | Register new user |
| POST | /api/auth/login | Login user |
| GET | /api/auth/me | Get current user |
| **Products** |||
| GET | /api/products | Get all products |
| GET | /api/products/active | Get active products |
| GET | /api/products/{id} | Get product by ID |
| GET | /api/products/category/{id} | Get products by category |
| GET | /api/products/search?term= | Search products |
| POST | /api/products | Create product |
| PUT | /api/products/{id} | Update product |
| DELETE | /api/products/{id} | Delete product |
| GET | /api/products/count | Get products count |
| **Categories** |||
| GET | /api/categories | Get all categories |
| GET | /api/categories/active | Get active categories |
| GET | /api/categories/{id} | Get category by ID |
| GET | /api/categories/{id}/products | Get category with products |
| POST | /api/categories | Create category |
| PUT | /api/categories/{id} | Update category |
| DELETE | /api/categories/{id} | Delete category |
| GET | /api/categories/count | Get categories count |
| **Orders** |||
| GET | /api/orders | Get all orders |
| GET | /api/orders/{id} | Get order by ID |
| GET | /api/orders/user/{userId} | Get user's orders |
| GET | /api/orders/status/{status} | Get orders by status |
| GET | /api/orders/pending | Get pending orders |
| POST | /api/orders | Create order |
| PUT | /api/orders/{id}/status | Update order status |
| PUT | /api/orders/{id}/confirm | Confirm order |
| PUT | /api/orders/{id}/ship | Ship order |
| PUT | /api/orders/{id}/deliver | Deliver order |
| PUT | /api/orders/{id}/cancel | Cancel order |
| GET | /api/orders/count | Get orders count |
| GET | /api/orders/pending/count | Get pending count |

---

## Running the API

```bash
# Navigate to solution root
cd OmanDigitalShopPlatformSolution

# Run the API
dotnet run --project Pll.Api.OmanDigitalShop

# API will be available at:
# https://localhost:7144/api
# Swagger UI: https://localhost:7144/swagger
```

---

**Version:** 1.0
**Last Updated:** 2024
**Author:** Joonguini - Programming in the Kitchen
