#include "DHT.h"

#define DHTPIN 2     // Пин D2
#define DHTTYPE DHT22 // Указываем, что у нас DHT22

DHT dht(DHTPIN, DHTTYPE);

void setup() {
  Serial.begin(9600); // Скорость COM-порта
  dht.begin();
}

void loop() {
  delay(2000); // Ждем 2 секунды

  float t = dht.readTemperatu re();
  float h = dht.readHumidity();

  // Проверка на ошибки чтения
  if (isnan(t) || isnan(h)) {
    Serial.println("ERROR");
  } else {
    // Отправляем строку в формате: "Температура,Влажность"
    Serial.print(t);
    Serial.print(",");
    Serial.println(h);
  }
}
