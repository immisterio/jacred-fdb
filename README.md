# Установка
curl -s https://raw.githubusercontent.com/immisterio/jac.red/main/install.sh | bash

# Источники 
Kinozal, Nnmclub, Rutor, Torrentby, Bitru, Rutracker, Megapeer, Selezen, Toloka (UKR), Rezka, Baibako, LostFilm, Anilibria, Animelayer, Anifilm

# Доступ к доменам .onion
1. Запустить tor на порту 9050
2. В init.conf указать .onion домен в host

# Настройка init.conf
* Список всех параметров, а так же значения по умолчанию смотреть в example.conf 
* В init.conf нужно указывать только те параметры, которые хотите изменить

```
{
  "listenport": 9120, // изменили порт
  "NNMClub": {        // изменили домен на адрес из сети tor 
    "alias": "http://nnmclub2vvjqzjne6q4rrozkkkdmlvnrcsyes2bbkm7e5ut2aproy4id.onion"
  },
  "globalproxy": [
    {
      "pattern": "\\.onion",  // запросы на домены .onion отправить через прокси
      "list": [
        "socks5://127.0.0.1:9050" // прокси сервер tor
      ]
    }
  ]
}
```
