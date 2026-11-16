# Arduino Sensor Monitor

Программа для мониторинга температуры и влажности в реальном времени.

## Стек:
* Микроконтроллер: Arduino Nano
* Датчик: DHT22
* ПО: C# (.NET)

## Как работает:
Arduino считывает данные с DHT22 и передает их по COM-порту. 
Программа на C# принимает данные, парсит их и рисует график в реальном времени (без использования сторонних библиотек, на чистом GDI+).

## Пример работы программы:
<img width="963" height="634" alt="image" src="https://github.com/user-attachments/assets/b5ccfbd4-7842-418e-a4f6-dddc09ad77a9" />
<img width="995" height="674" alt="image" src="https://github.com/user-attachments/assets/ddda615e-bf13-4de3-8bbc-6945c95abe6e" />

