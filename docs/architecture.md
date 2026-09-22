# Карта архітектури

Це карта архітектури вебсистеми «Трекер інцидентів», доповнена під час виконання ЛР 1 власним трасуванням запиту, конкретними файлами та спостереженнями з DevTools і журналів.

Компоненти

| Компонент | Розташування | Відповідальність |
|---|---|---|
| Browser client | `src/SecureLab.Api/client/` | Надсилає HTTP-запити, безпечно показує відповідь через DOM API |
| Presentation | `src/SecureLab.Api/Presentation/` | Описує endpoints, читає зовнішні параметри, формує HTTP-відповідь |
| Application | `src/SecureLab.Api/Application/` | Виконує сценарій отримання списку, деталей або підсумку інцидентів |
| Data | `src/SecureLab.Api/Data/` | Відображає C#-сутності на PostgreSQL через EF Core/Npgsql |
| PostgreSQL | `infra/compose.yaml` | Зберігає навчальні дані у локальному контейнері |

Підготовлений наскрізний маршрут


Клік на «Показати підсумок» у client/app.js
  → GET /api/incidents/severity-summary
  → Presentation/Endpoints/IncidentEndpoints.cs (метод GetSeveritySummaryAsync)
  → Application/Incidents/IncidentQueries.cs (метод GetSeveritySummaryAsync)
  → Data/SecureLabDbContext.cs (LINQ GroupBy та AsNoTracking)
  → PostgreSQL (виконання SQL: SELECT i.severity, COUNT(*)::INT FROM incidents GROUP BY i.severity)
  → response DTO у Presentation/Contracts/IncidentResponses.cs (IncidentSeveritySummaryResponse)
  → JSON (код 200 OK з масивом згрупованих даних)
  → textContent / createTextNode у client/app.js (безпечний рендеринг у #severity-summary-list)

  Межі довіри

Користувач -> Browser client

Ризик: Користувач повністю контролює введення даних через елементи DOM та консоль розробника.

Захист: Валідація вхідних даних на боці клієнта та обмеження полів формами.

Browser client -> API

Ризик: HTTP-запити можна легко перехопити, підробити або надіслати в обхід інтерфейсу (наприклад, через cURL чи Postman).

Захист: Перевірка даних на бекенді в Minimal API ендпоінтах із використанням строго типізованих моделей та DTO.

PostgreSQL -> API -> DOM

Ризик: У базі даних може зберігатися раніше введений недовірений текст із тегами чи скриптами (Stored XSS).

Захист: Безпечна передача через DTO та використання суто безпечних методів DOM API (textContent та createTextNode) замість innerHTML у файлі app.js.
Конфігураційні входи
global.json - версія .NET SDK;

src/SecureLab.Api/appsettings*.json - режим міграцій і локальний connection string;

infra/compose.yaml - версія PostgreSQL, порт і локальні навчальні облікові дані;

змінна середовища ConnectionStrings__SecureLab - безпечний спосіб перевизначити connection string поза репозиторієм.