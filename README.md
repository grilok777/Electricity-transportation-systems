# ⚡ Distributed Energy Resource Management System (Облікова система енергоресурсів)

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![React](https://img.shields.io/badge/React-18.x-61DAFB?style=for-the-badge&logo=react&logoColor=black)
![TypeScript](https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white)
![MS SQL Server](https://img.shields.io/badge/MS_SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS_v4-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white)

## 📖 Про проєкт

Комплексна розподілена облікова система для управління фізичними активами електромережі, ведення реєстру клієнтів (угод) та прогнозування генерації електроенергії. Проєкт демонструє побудову мікросервісної архітектури, де дві незалежні системи обмінюються даними через REST API у фоновому режимі.

Проєкт складається з двох основних підсистем:
1. **Grid System (Панель Оператора)** — управління інфраструктурою (підстанції, лінії), реєстрація угод, розрахунок балансу мережі та модерація прогнозів.
2. **Generation System (Кабінет Клієнта)** — управління власними електростанціями (сонячні, вітрові) та подача погодинних прогнозів генерації.

## 🏗 Ключові архітектурні рішення

* **CQRS Pattern:** Логіка читання та запису розділена за допомогою бібліотеки **MediatR**, що забезпечує слабку зв'язність (loose coupling) та легке масштабування коду.
* **Синхронізація розподілених баз даних:** Реалізовано механізм асинхронної двосторонньої синхронізації (Background Workers), який періодично опитує суміжні REST API ендпоінти та оновлює дані в локальній БД (з вирішенням конфліктів за `LastModifiedAt`).
* **Repository & Unit of Work:** Патерни використані для інкапсуляції логіки роботи з Entity Framework Core та забезпечення транзакційності операцій.
* **Автентифікація:** Безпека реалізована за допомогою JWT-токенів (з кастомним хешуванням паролів через BCrypt).
* **Вирішення Multiple Cascade Paths:** Архітектура БД спроєктована з урахуванням обмежень MS SQL Server щодо циклічного каскадного видалення.

## 🛠 Технологічний стек

**Backend:**
* C#, .NET 8, ASP.NET Core Web API
* Entity Framework Core (Code-First)
* MS SQL Server / T-SQL
* MediatR (CQRS)
* BCrypt.Net (Криптографія)
* JWT Authentication

**Frontend:**
* React, Vite
* TypeScript
* Tailwind CSS v4
* Axios
* React Router DOM

## 🚀 Як запустити проєкт локально

### 1. Налаштування Баз Даних
Переконайтеся, що у вас встановлено MS SQL Server (LocalDB або повноцінний).
У файлах `appsettings.json` обох бекенд-проєктів (`Grid.WebAPI` та `Generation.WebAPI`) перевірте Connection Strings:
``json
"ConnectionStrings": {
  "GridDatabase": "Server=(localdb)\\mssqllocaldb;Database=GridSystemDb;Trusted_Connection=True;",
  "GenerationDatabase": "Server=(localdb)\\mssqllocaldb;Database=GenerationSystemDb;Trusted_Connection=True;"
}
2. Застосування міграцій
Відкрийте Package Manager Console у Visual Studio, оберіть інфраструктурний проєкт і виконайте для кожної системи:

PowerShell
Update-Database
3. Запуск Backend-сервісів
Запустіть обидва проєкти WebAPI одночасно. Переконайтеся, що порти у фонових воркерах (GenerationToGridSyncWorker.cs тощо) збігаються з реальними портами, які видала Visual Studio (наприклад, https://localhost:7100 та https://localhost:63716).

4. Запуск Frontend-додатків
Відкрийте два термінали для адмінки оператора та клієнтської частини:

Bash
# Для кожної папки фронтенду виконайте:
npm install
npm run dev
📂 Структура проєкту
/Application — Бізнес-логіка, DTO, MediatR Commands/Queries, FluentValidation.

/Domain — Основні сутності (Entities), Enum, Інтерфейси репозиторіїв.

/Infrastructure — Контекст БД (EF Core), реалізація репозиторіїв, сервіси хешування та генерації JWT.

/WebAPI — Контролери, Middleware, налаштування DI (Dependency Injection), Background Фонова синхронізація.

/Frontend — React-компоненти, хуки, сервіси для API-запитів.

Розроблено як демонстрацію навичок побудови надійних облікових систем.
