# EksiSozluk

# Introduction
This project aims to create a structure similar to the popular social platform Eksisozluk. I have used Onion Architecture to create a layered architecture. Users' various operations are sent to the RabbitMQ queue, processed by Worker Services, and stored in the database using Dapper and Entity Framework. Redis is used to cache infrequently changing data for fast access. Additionally, CQRS, Unit of Work (UoW), and Repository design patterns are employed.

# Architecture
### Onion Architecture
Onion Architecture is an architectural pattern that aims to separate the core business logic of an application into independent and replaceable layers. The primary advantages of this architecture include controlled dependencies, modular structure, and testability.

# Messaging and Queueing
RabbitMQ and Worker Services
RabbitMQ is a powerful message queue system that allows messages to be queued and processed asynchronously. In my project, 'users' operations such as 'entryVote', 'EntryFavorite', 'CommentFavorite', 'Email' and 'Password' changes are sent to the RabbitMQ queue and processed by Worker Services like FavoriteService, UserService, and VoteService. This ensures that operations are performed quickly and efficiently.


![rabbitmq](https://github.com/user-attachments/assets/7d37cfc5-d2ef-49a0-affe-c3bc919b1808)


# Database Operations
### Dapper and Entity Framework
For database operations, I have used both Dapper and Entity Framework.

Dapper:When communicating with RabbitMQ, it was used within the Worker Service for faster performance. Dapper is a lightweight and fast ORM (Object-Relational Mapper) for .NET applications. It executes SQL queries directly, providing performance advantages and performing database operations quickly.


# Caching
### Redis
Redis is a high-performance, in-memory data structure store. In my project, infrequently changing user data is cached in Redis to prevent unnecessary database queries and improve performance. This ensures fast access to user information.

# CQRS, UoW and Repository Design Pattern
CQRS is a design pattern that separates command and query operations, enhancing application performance and scalability. This pattern handles data reading and writing operations in separate layers.

### Unit of Work (UoW)
Unit of Work is a design pattern that keeps database operations together, ensuring that all operations either succeed or fail as a whole. This pattern is used to ensure data consistency.

### Repository Design Pattern
The Repository Design Pattern abstracts database operations, making the data access layer more manageable and testable. This pattern decouples database operations from other layers of the application.

## Installation and Usage

To clone the project to your local environment:

```bash
git clone https://github.com/BatuhanKayaoglu/EksiSozluk-API.git
