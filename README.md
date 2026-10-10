# OrderApi

REST API для интернет-магазина на ASP.NET Core Web API.

## Стек

- C# / .NET 10
- ASP.NET Core Web API — контроллеры, DI
- Entity Framework Core — ORM, миграции
- SQL Server — база данных

## Функциональность

- CRUD для товаров и заказов
- Связь многие-ко-многим с дополнительным полем (Order ↔ Product через OrderItem с Quantity)
- Расчёт общей суммы заказа и количества товаров
- DTO для запросов и ответов
- Валидация входных данных (Data Annotations)
- Swagger UI для тестирования

## Эндпоинты

### Products

| Метод | URL | Описание |
|-------|-----|----------|
| GET | /api/products | Все товары |
| GET | /api/products/{id} | Товар по Id |
| POST | /api/products | Создать товар |
| PUT | /api/products/{id} | Обновить товар |
| DELETE | /api/products/{id} | Удалить товар |

### Orders

| Метод | URL | Описание |
|-------|-----|----------|
| GET | /api/orders | Все заказы с суммой и количеством товаров |
| GET | /api/orders/{id} | Заказ по Id |
| POST | /api/orders | Создать заказ с позициями |
| PUT | /api/orders/{id} | Обновить заказ |
| DELETE | /api/orders/{id} | Удалить заказ с позициями |
| GET | /api/orders/{id}/items | Позиции заказа |

## Архитектура

- Models — сущности БД (Product, Order, OrderItem)
- DTOs — контракты API
- Data — DbContext
- Controllers — обработка HTTP-запросов

## Как запустить

1. Установить .NET 10 SDK и SQL Server Express.
2. Клонировать репозиторий: git clone https://github.com/danila-markevich/OrderApi.git
3. Настроить строку подключения в Program.cs.
4. Применить миграции: dotnet ef database update
5. Запустить: dotnet run
6. Открыть Swagger: https://localhost:XXXX/swagger

## Автор

Danila Markevich — https://github.com/danila-markevich