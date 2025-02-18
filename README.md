##Проект с использованием микросервисной архитектуры

![Схема](https://github.com/tsynlun21/Microservices/blob/master/sceme.png)

## Стек
* ASP .NET 8
* EF.Core
* RabbitMq
* PostgreSQL
* MongoDb
* Masstransit
* Ocelot

## Микросервисы

### Identity
  Микросервис выполняет функцию аутентификации пользователя, используя библиотеку AspNetCore.Identity, при регистрации/авторзиации пользователя возвращается JWT токен, который в дальнейшем нужно передававать в определенные запросы
