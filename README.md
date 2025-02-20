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

* Identity
* Showroom
* Purchases
* CarHistory

### Identity
  > Порт - 5001
  Микросервис выполняет функцию аутентификации пользователя, используя библиотеку AspNetCore.Identity.

  
  
  #### Методы
  ```Http
  Post /auth/login
  ```
  Осуществляет вход в систему пользователем, возвращает модель пользователя, в которой из важного 
  - роль пользователя (admin, merchant, buyer)
  - токен
Токен и роль используются для получения доступа к определенным методам/сервисам

***по умолчанию предзагружен один пользователь - администратор, выполнив вход под его учеткой, появляется доступ к "приватным" методам, - Post auth/set-role, Post /carhistory***

```javascript
  {
    "username" : "admin",
    "password" : "gigachad"
  }
```

  **пример ответа**
  ```javascript
    {
        "id": "301573c6-f0bf-4c4e-bccf-ed8e6b98e266",
        "roles": [
            "admin"
        ],
        "email": "admin@admin.com",
        "userName": "ADMIN",
        "phoneNumber": null,
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjMwMTU3M2M2LWYwYmYtNGM0ZS1iY2NmLWVkOGU2Yjk4ZTI2NiIsInVuaXF1ZV9uYW1lIjoiQURNSU4iLCJuYW1laWQiOiIzMDE1NzNjNi1mMGJmLTRjNGUtYmNjZi1lZDhlNmI5OGUyNjYiLCJyb2xlIjoiYWRtaW4iLCJuYmYiOjE3NDAwNTMxNjIsImV4cCI6MTc0MDA4OTE2MiwiaWF0IjoxNzQwMDUzMTYyfQ.LAtbroJf4V_FT_jETME81fucFPtkfwWD-DkG94Qk0j8"
    }
  ```
  

  ```Http
  Post /auth/register
  ```
  Регистрация нового пользователя с ролью Buyer

  **пример тела запроса**
  ```
  {
    "username" : "chelik",
    "password" : "cooolpass123",
    "email" : "mineralnyevody@obratno.ru",
    "phone" : "82286661337"
  }
```

  ```Http
  Post /auth/set-role 
  ```
  **Admin**
  Установка новой роли пользователя

  **пример тела запроса**
  {
    "userid" : "634db3ff-ab07-4e11-9b20-614f3f4279fe",
    "role" : "merchant"
  }

### Purchases
  > Порт : 5002
  Микросервис получения/заполнения информации о покупках

  **все методы доступны только пользователям, прошедшим аутентификацию**
  #### Методы

  ```Http
    Get /purchases/get-transactions
  ```

  Возвращает список транзакций текущего пользователя

  ```
    Get /purchases/get-transaction-by-id/{id}
  ```

  Получить транзакцию по ее id

  ```
    Put /purchases/update-transaction
  ```
  **Admin || Merchant**

  Изменить транзакцию пользователя

  **пример тела запроса**

  ```
    {
    "id": 3,
    "date": "2025-02-20T12:54:37.094Z",
    "vehicle": {
      "vehicleId": 2,
      "vin": "SOME-car-Vin-CoDe-78910",
      "brand": "Vaz",
      "model": "Vaz 2107",
      "releaseDate": "1998-02-20",
      "color": 1,
      "price": 250000,
      "mileage": 150000
    },
    "extraItems": [
      {
        "name": "DL Audio sabwoofer",
        "type": 1,
        "vehicleModel": "Vaz 2107",
        "count": 1,
        "price": 30000
      }
    ],
    "type": 1
    }
  ```

  В транзакции можно изменить покупаемый автомобиль и дополительные товары к нему, так же можно указать тип оплаты - 0 - наличка, 1 - карта

  **Метода, позволяющего добавить тразнакцию вручную апи не предоставляет, поскольку вся логика добавления транзакции выполняется автоматически при совершении покупки**
  
### Showroom
> Порт 5003

Микросервис, который позволяет 
- получить данные о имеющихся автомобилях в автосалоне,
  - добавить автомобили в автосалон
- получить список автомобилей с определенного производетеля (*опционально* модели)
- получить список автомобилей в ценовом диапазоне (min < цена_авто  < *опционально*max)
- получить список доступных дополнительных товаров для модели авто
- **Admin || Merchant** добавить автомобили в салоны
- **Admin || Merchant** добавить дополнительные товары для определенной модели автомобиля
- **Buyer** купить автомобиль и *опционально* дополнительные товары к нему



```
  Get /showroom/vehicles-by-showroom/{id}
```

Получаем список автомобилей по ```id``` автосалона

**пример ответа**

```
[
        {
            "vehicleId": 1,
            "vin": "23D34EC8-F83F-4BED-95D6-05BDE4F76222",
            "brand": "BMW",
            "model": "BMW 325",
            "releaseDate": "2015-01-01",
            "color": 4,
            "price": 2350000.0,
            "mileage": 120000.0
        },
        {
            "vehicleId": 2,
            "vin": "6786D736-1C1D-4375-8A35-6594E5133823",
            "brand": "BMW",
            "model": "BMW 325",
            "releaseDate": "2017-06-13",
            "color": 3,
            "price": 3200000.0,
            "mileage": 85000.0
        },
        {
            "vehicleId": 3,
            "vin": "8E10D3F9-FE4A-4023-9E44-505249AE95C5",
            "brand": "BMW",
            "model": "BMW M5 F90",
            "releaseDate": "2020-11-01",
            "color": 3,
            "price": 10200000.0,
            "mileage": 20000.0
        }
    ]
```

```Color``` 
1 - Красный
2 - Белый
3 - Голубой
4 - Серый
5 - Серебристый
6 - Зеленый

```
Post /showroom/vehicles-by-brand-and-model/
```

**пример запроса**

```
{
  "showroomId": 2,
  "brand": "Mercedes-Benz",
  "model": ""
}
```

Здесь можно опционально не указывать модель, тогда вернется список всех авто марки = ```brand```

```
Get /showroom/extra-parts-for-model/{model}
```

Получаем список дополнительных товаров для определенной модели авто

**пример ответа**

```
[
    {
      "name": "Harman Kardon audio system",
      "type": 1,
      "vehicleModel": "BMW 325",
      "count": 10,
      "price": 185000
    },
    {
      "name": "Lip spoiler",
      "type": 2,
      "vehicleModel": "BMW 325",
      "count": 10,
      "price": 45000
    },
    {
      "name": "LED lights",
      "type": 1,
      "vehicleModel": "BMW 325",
      "count": 10,
      "price": 310000
    }
  ]
```

```
Post /showroom/vehicles-in-price
```

Получаем список авто в ценовом диапазоне

**пример запроса**

```
{
  "maxPrice": ,
  "minPrice": 0
}
```

В запросе обязательно нужно указывать только минимальный порог


```
Post /showroom/add-vehicles
```

Добавляем автомобили в автосалоны


**пример запроса**

```
  {
    "vehiclesPerShowrooms" : [
        {
            "showroomid" : 1,
    "vehicles" : [
        {
            "vin" : "A803358B-8495-49F3-AE0D-244D1C9CD833",
            "brand" : "Mercedes-Benz",
            "model" : "CLS63 AMG",
            "releaseDate" : "2021-03-08",
            "color" : 3,
            "price" : 13125000,
            "mileage" : 35000
        }
      ]
        }
    ]
  }
```

*При добавлении автомобиля публикуется сообщение с vin нового авто, это сообщение обрабатывается консюмером в сервисе CarHistory, который генерирует информацию о автомобиле и сохраняет в базу*



```
Post /showrooms/add-extra-parts
```

Добавляем дополнительные товары для определенной модели авто

**пример запроса**
```
{
  "extraItems": [
    {
      "name": "DL Audio sabwoofer",
      "type": 1,
      "vehicleModel": "Vaz 2107",
      "count": 1,
      "price": 30000
    }
  ]
}
```

```
Post /showrooms/buy
```
**Buyer**

Совершаем покупку автомобиля и дополнительных товаров для него

**пример запроса**

```
{
  "showroomId": 0,
  "purchase": {
    "vehicleId": 3,
    "extraItems": [
      {
        "name": "Bower&Wilkins audio system",
        "count": 1
      }
    ]
  }
}
```

*При совершении покупки создается и сохраняется чек, также публикуется сообщение для сервиса покупок на добавление транзакции*

### Car history

Микросервис который позволяет просмотреть информацию о автмобилю

```
Get /car-history/
```

Получаем информацию о автомобиле

```
{
    "carInfo": {
      "generalInfoDescription": "Автомобиль не участвовал в ДТП.\n",
      "juridicalInfoDescription": "Юридический статус: Чистый\nКоличество владельцев: 1\nПробег в пределах нормы.\n"
    },
    "vin": "A803358B-8495-49F3-AE0D-244D1C9CD833",
    "brand": "Mercedes-Benz",
    "model": "CLS63 AMG",
    "color": "Blue",
    "releaseDate": "2021-03-08",
    "mileage": 35000
  }
```

```
Post /car-history/
```
**Admin**

Добавить инфомрацию о автомобиле


## Базы данных

В проекте используется 4 базы данных

* Identity, Showroom, Purchases - PostgreSQL
* CarHistory - MongoDB

> ORM: Entity Framework Core
> Подход: Code first
> Базы развернуты в докере

## RabbitMQ 
> Занимает стандартные порты 15672 и 5672
>
Используется в связке с Masstransit
*время ожидание ответа от Masstransit.RespondAsync для Debug сборки увеличено до 1 часа для удобства дебага*

## Ocelot 
> Порт 5000
>
Добавлен Api Gateway для маршрутизации запросов, кеширования и проверки прав доступа
Для ```/showroom/vehicles-by-showroom/{id}``` установлен лимит запросов по времени и добавлено кеширование данных

#### Все взаимодействие подразумевается через апи шлюз, в [конфиге](https://github.com/tsynlun21/Microservices/blob/master/ApiGateway/ocelot.json) можно подробнее узнать эндпоинты для взаимодействия

## Docker
Проект поддерживает контейниризацию, в корне проекта есть ```docker-compose``` файл, с конфигурацией для поднятия проекта.



  
