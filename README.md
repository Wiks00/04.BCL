# Задания к модулю BCL
## Общее

В данном задании нам потребуется написать приложение-службу (консольное приложение), которое будет:
1.	Слушать несколько входящих папок
2.	При появлении в папке файла
    * Пробегать по внутреннему списку правил, который состоит из пары «шаблон имени файла – папка назначения». 
    * При совпадении шаблона перекладывать файл
    * Если нужного шаблона не найдено – перекладывать в папку «по умолчанию»
3. Логгировать на экране свою деятельность. В частности, такие события как:
    * Обнаружение нового файла (его имя и дата создания)
    *	Нахождение/не нахождение подходящего правила
    * Перенос файла в папку назначения
## Задание 1. Локализация приложения
Разрабатываемое приложение необходимо сделать максимально интернационализуемым:
* Использовать установку требуемой культуры при старте приложения (см. задание 2)
*	Все строки перенести в ресурсы
* Использовать форматирование дат и других типов данных.
Для демонстрации разработайте как минимум 2 локализации:
    *	Русская
    *	Английская
## Задание 2. Конфигурирование
Приложение должно уметь читать свои стартовые настройки из стандартного application.config. Для задания настроек создать собственную configuration section, в которой максимально использовать подходящие типы данных (т.е. не использовать просто строки!).
В качестве настроек должно быть указано следующее:
1.	Требуемую культуру для интерфейса
2.	Список папок, которые нужно слушать
3.	Список правил, который включает в себя
    *	Шаблон имени файла в виде регулярного выражения
    *	Папка назначения
    *	Параметры того, как меняется имя выходного файла:
    *	Добавлять ли порядковый номер
    *	Добавлять ли дату перемещения
