# TaskManager API

Простой REST API для управления задачами (to-do list), написанный на ASP.NET Core.
Делал его, чтобы разобраться с Minimal API, EF Core и в целом понять, как строится Web API с нуля — без готовых шаблонов.

## Стек

- ASP.NET Core 10, Minimal API
- EF Core + SQLite
- Data Annotations для валидации
- Swagger (для тестирования эндпоинтов без Postman)


## Как запустить

```bash
cd TaskManager.Api
dotnet restore
dotnet run
```

Swagger откроется на `http://localhost:5158/swagger` (порт может отличаться, смотри в консоли при запуске).
База SQLite создаётся сама при первом старте, миграций пока нет — использую `EnsureCreated()`, для пет-проекта этого достаточно.

## Эндпоинты

- `GET /api/tasks` — список задач, можно фильтровать по `?status=InProgress`
- `GET /api/tasks/{id}` — одна задача
- `POST /api/tasks` — создать
- `PUT /api/tasks/{id}` — обновить (полностью, статус обязателен)
- `DELETE /api/tasks/{id}` — удалить

Статусы: `ToDo`, `InProgress`, `Done`.

## Пример запроса

```bash
curl -X POST http://localhost:5158/api/tasks \
  -H "Content-Type: application/json" \
  -d '{ "title": "Изучить Minimal API", "description": "Разобраться с маршрутизацией", "dueDate": "2026-08-01" }'
```

## Что можно ещё доделать

- Пагинация в списке задач
- Тесты (пока не писал, следующим шагом хочу разобраться с xUnit)
- Нормальные миграции вместо EnsureCreated
