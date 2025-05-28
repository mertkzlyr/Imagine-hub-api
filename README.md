# ImagineHub API

A sophisticated social media platform backend that combines social networking features with AI-powered image generation capabilities. Built with ASP.NET Core 9.0, this API provides a robust foundation for a modern social media application.

## 🚀 Features

- **User Management**
  - Secure authentication with JWT
  - User registration and profile management
  - Role-based access control (User/Admin)
  - Profile picture support

- **Social Features**
  - Post creation and management
  - Comment system with nested replies
  - Like system for posts and comments
  - Follow/unfollow functionality

- **AI Image Generation**
  - Integration with AI image generation service
  - Token-based generation system
  - Image optimization and storage
  - WebP format support

- **Performance & Scalability**
  - Redis caching for frequently accessed data
  - Optimized database queries
  - Pagination for large data sets
  - Efficient image handling

## 🛠️ Technology Stack

- **Backend Framework**: ASP.NET Core 9.0
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Caching**: Redis
- **Authentication**: JWT with RSA
- **Image Processing**: ImageSharp
- **Password Hashing**: BCrypt
- **API Documentation**: Swagger/OpenAPI

## 📋 Prerequisites

- .NET 9.0 SDK
- SQL Server
- Redis Server
- Docker (optional)

## 🔧 Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/ImagineHubAPI.git
   cd ImagineHubAPI
   ```

2. **Configure the database**
   - Update the connection string in `appsettings.json`
   - Run the database migrations

3. **Set up Redis**
   - Install Redis server
   - Update Redis connection string in `appsettings.json`

4. **Configure JWT Keys**
   - Generate RSA key pair
   - Place the keys in the project root:
     - `private.key`
     - `public.key`

5. **Install dependencies**
   ```bash
   dotnet restore
   ```

6. **Run the application**
   ```bash
   dotnet run
   ```

## 🐳 Docker Support

Build and run using Docker:

```bash
docker build -t imaginehubapi .
docker run -p 8080:8080 imaginehubapi
```

## 📚 API Documentation

Once the application is running, you can access the Swagger documentation at:
```
https://localhost:8080/swagger
```

## 🔐 Security Features

- JWT-based authentication
- RSA asymmetric encryption
- Password hashing with BCrypt
- Role-based authorization
- HTTPS enforcement in production
- Secure file handling

## 🏗️ Project Structure

```
ImagineHubAPI/
├── Config/                 # Configuration classes
├── Controllers/           # API endpoints
├── Data/                  # Database context
├── DTOs/                  # Data transfer objects
├── Helpers/              # Utility classes
├── Interfaces/           # Service interfaces
├── Middlewares/          # Custom middleware
├── Models/               # Entity models
├── Repositories/         # Data access layer
├── Services/             # Business logic
└── wwwroot/             # Static files
```

## 🔄 API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - User login

### Users
- `GET /api/users/{id}` - Get user profile
- `PUT /api/users/update` - Update user profile
- `DELETE /api/users/delete-account` - Delete account

### Posts
- `POST /api/posts` - Create new post
- `GET /api/posts` - Get all posts
- `GET /api/posts/{id}` - Get post by ID
- `PUT /api/posts/{id}` - Update post
- `DELETE /api/posts/{id}` - Delete post

### Images
- `POST /api/images/generate-image` - Generate AI image
- `GET /api/images/{id}` - Get image by ID
- `GET /api/images` - Get user's images

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Authors

- MERT KIZILYAR
