API<br/>
Аутентификация по jwt-токену. Работает только у приложения Клиента. 
По пользователям. Храним в базе логин, хешированный пароль, соль, id склада пользователя. Предполагается что пользователь — это Аптека которая хранит продукты.
Добавлены склады для хранения продуктов. Модель Stock хранит id склада, id продукта и количество продукта. Клиент авторизуюсь получает id склада, закреплённого за ним и все запросы на сервер, делает с учетом этого id.

Приложение Клиента<br/>
Добавлена аутентификация. Запрос к серверу чтобы получить товары только его склада. 

Приложение Администратора<br/>
Добавлены стили элементов в App.xaml.
В редактирование продукта также добавлена возможность его добавления.
Создан интерфейс.Все методы по взаимодействию с API вынесены в отдельный класс. При инициализации Конструктор класса EditViewModel принимает этот интерфейс. При создании главного окна типизируем EditViewModel классом который реализует интерфейс IProductAPI.<br/>

При открытии окна запускаем в фоне таску для того чтобы получить имена продуктов. Храним имена в HashSet<string> для быстрого поиска. При добавлении продукта сперва проверяем есть у нас такой продукт или нет. Если нет, то отправляем запрос на сервер.
Добавлены оповещения о добавлении/изменении продукта<br/>

Не сделано: <br/>
•	Логика по продаже продукта. По отправке/приемка товара со склада в аптеку.<br/>
•	Окно администратора для просмотра содержимого по складам<br/>
•	Цена по-прежнему хранится не в decimal<br/>
•	Нет ограничений на размер полей в базе.<br/>
•	Файл конфигурации для хранения адреса сервера и бд.<br/>
•	Визуальная часть.
