# TaskManager API

Учебный REST API на ASP.NET Core (Minimal API) для управления задачами.
Основная цель проекта — изучение построения Web API, маршрутизации, работы с HTTP-запросами и подключения к базе данных через EF Core.

## Технологии

- ASP.NET Core 8.0 (Minimal API)
- Entity Framework Core + SQLite
- Swagger / OpenAPI (Swashbuckle)
- Data Annotations (валидация)

## Структура проекта

```
TaskManager.Api/
├── Data/
│   └── AppDbContext.cs        # контекст базы данных
├── Dtos/
│   ├── TaskItemDto.cs         # DTO для ответа
│   ├── CreateTaskItemDto.cs   # DTO для создания задачи (с валидацией)
│   └── UpdateTaskItemDto.cs   # DTO для обновления задачи (с валидацией)
├── Endpoints/
│   └── TaskEndpoints.cs       # маршруты CRUD для задач
├── Models/
│   ├── TaskItem.cs            # сущность задачи
│   └── Enums/TaskItemStatus.cs
└── Program.cs                 # точка входа, конфигурация сервисов
```

## Запуск проекта

```bash
cd TaskManager.Api
dotnet restore
dotnet run
```

После запуска Swagger UI откроется автоматически по адресу вида `http://localhost:5158/swagger`.
База данных SQLite (`taskmanager.db`) создаётся автоматически при первом запуске.

## Эндпоинты

| Метод  | Маршрут                        | Описание                                  |
|--------|---------------------------------|--------------------------------------------|
| GET    | `/api/tasks`                    | Список всех задач                          |
| GET    | `/api/tasks?status=InProgress`  | Список задач с фильтром по статусу         |
| GET    | `/api/tasks/{id}`               | Получить задачу по id                      |
| POST   | `/api/tasks`                    | Создать новую задачу                       |
| PUT    | `/api/tasks/{id}`               | Обновить задачу                            |
| DELETE | `/api/tasks/{id}`               | Удалить задачу                             |

Статусы задачи: `ToDo`, `InProgress`, `Done`.

## Пример запроса на создание задачи

```bash
curl -X POST http://localhost:5158/api/tasks \
  -H "Content-Type: application/json" \
  -d '{ "title": "Изучить Minimal API", "description": "Разобраться с маршрутизацией", "dueDate": "2026-08-01" }'
```

Пример ответа:

```json
{
  "id": 1,
  "title": "Изучить Minimal API",
  "description": "Разобраться с маршрутизацией",
  "status": "ToDo",
  "createdAt": "2026-07-26T10:00:00Z",
  "dueDate": "2026-08-01T00:00:00Z"
}
```

## Возможные улучшения (roadmap)

- Пагинация для списка задач
- Unit-тесты (xUnit + WebApplicationFactory)
- Аутентификация (JWT)
- Миграции EF Core вместо `EnsureCreated()`
- Docker-контейнер
