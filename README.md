# UsersAPI

Для запуска проекта нужно:

- запустить СУБД и подключить сервер
- в UsersAPI в файле appsettings.Development.json заменить существующую ConnectionString на свою
- через Package Manager Console внести изменения в БД через update-database (для консоли должен быть выбран DAL)

Готово!

В качестве дальнейших улучшений: добавление хеширования пароля перед сохранением в БД, реализация ILogger.
