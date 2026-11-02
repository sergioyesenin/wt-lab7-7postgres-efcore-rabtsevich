Метод	URL	Назначение	Статусы
GET	/	Проверка сервиса	200
GET	/api/info	Информация о приложении	200
GET	/api/flights	Список рейсов (query search)	200
GET	/api/flights/{id}	Рейс по ID	200, 404
POST	/api/flights	Создать рейс	201, 400
PUT	/api/flights/{id}	Изменить рейс	200, 400, 404
DELETE	/api/flights/{id}	Удалить рейс	204, 404
GET	/api/airlines	Справочник авиакомпаний	200
GET	/api/aircrafts	Справочник ВС	200