![build](https://github.com/planara/planara-projects/actions/workflows/build.yml/badge.svg)
![deploy](https://github.com/planara/planara-projects/actions/workflows/deploy.yml/badge.svg?branch=main)
![version](https://img.shields.io/github/v/tag/planara/planara-projects?sort=semver)
[![Codecov](https://codecov.io/gh/planara/planara-projects/branch/main/graph/badge.svg)](https://codecov.io/gh/planara/planara-projects)

## Planara.Projects

Сервис управления пользовательскими проектами.

Отвечает за создание, хранение, получение, обновление и удаление проектов пользователя
(название, описание, ссылка на файл проекта, приватность и дата обновления).

Каждый проект принадлежит конкретному пользователю и доступен только владельцу.

Реализован как ASP.NET Core + GraphQL сервис с JWT-аутентификацией.

## Features

- Создание проектов
- Получение проекта по ID
- Получение списка проектов пользователя
- Пагинация списка проектов
- Получение общего количества проектов (`totalCount`)
- Фильтрация и сортировка проектов
- Частичное обновление проекта
- Удаление проекта
- Проверка владельца проекта через `UserId`
- JWT авторизация (`[Authorize]`)
- Валидация входных данных (FluentValidation)
- GraphQL API (HotChocolate)

## GraphQL API

### Queries

- `getProject(request: GetProjectByIdRequest): Project`  
  Возвращает проект текущего пользователя по ID  
  _(требует авторизации)_

- `getMyProjects: ProjectConnection`  
  Возвращает список проектов текущего пользователя  
  Поддерживает пагинацию, фильтрацию, сортировку и получение `totalCount`  
  _(требует авторизации)_

### Mutations

- `createProject(request: CreateProjectRequest): Project`  
  Создает новый проект для текущего пользователя  
  _(требует авторизации)_

- `updateProject(request: UpdateProjectRequest): Project`  
  Обновляет проект текущего пользователя  
  Поддерживает частичное обновление (обновляются только переданные поля)  
  _(требует авторизации)_

- `deleteProject(request: DeleteProjectRequest): DeleteProjectResponse`  
  Удаляет проект текущего пользователя  
  _(требует авторизации)_
