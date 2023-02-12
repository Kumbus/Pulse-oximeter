#include <WebConfig.h>
#include <ConfigManager.h>

WebConfig::WebConfig() {
  server.begin();
}

String WebConfig::prepareHtmlPage(String htmlContent) {
  String htmlPage =
     String("HTTP/1.1 200 OK\r\n") +
            "Content-Type: text/html; charset=utf-8\r\n" +
            "Connection: close\r\n" +
            "\r\n" +
            "<!DOCTYPE HTML>" +
            "<html>" +
            htmlContent +
            "</html>" +
            "\r\n";
  return htmlPage;
}

String WebConfig::prepareNetworksForm() {
  String content =
    String("<p>Wybierz z poniższej listy nazwę sieci WiFi, do której urządzenie ma się podłączyć.</p>") +
          "<form action='.' method='POST'>" +
          "  <label for='ssid'><p>Nazwa sieci</p></label>" +
          "  <p><input id='ssid' type='text' name='ssid'/></p>" +
          "  <label for='password'><p>Hasło (opcjonalnie)</p></label>" +
          "  <p><input id='password' type='password' name='password'/></p>" +
          "  <input type='submit' value='Zapisz ustawienia'/>" +
          "</form>";
  return content;
}


void WebConfig::listen() {
  WiFiClient client = server.available();
  if(client) {
    bool isRequestProcessed = false;
    String line;
    int responseType = -1;
    while(client.connected()) {
      for(int x = 0; client.available(); x++) {
        isRequestProcessed = true;
        line = client.readStringUntil('\r');
        Serial.println(line);
        line.trim();
        Serial.println(line);
        if(x == 0) {
          if(line.startsWith("GET")) {
            responseType = 0;
          } else if(line.startsWith("POST")) {
            responseType = 1;
          }
        }
      }
      
      if(isRequestProcessed) {
        switch(responseType) {
          case 0:
            client.println(prepareHtmlPage(prepareNetworksForm()));
            break;
          case 1:
            if(decodeAndSaveData(line)) {
              client.println(prepareHtmlPage("Dane konfiguracyjne zostaly zapisane"));
            } else {
              client.println(prepareHtmlPage("Wystąpił błąd podczas zapisywania danych"));
            }
            break;
          default:
            client.println(prepareHtmlPage("Nie rozpoznano polecenia"));
        }
        break;
      }
    }
    delay(1);
    client.stop();
  }
}

bool WebConfig::decodeAndSaveData(String data) {
  String ssid = "", pass = "";
  data.replace("+", " ");
  String field = "";
  for(unsigned int x = 0; x < data.length(); x++) {

    if(data[x] == '&' || x == data.length() - 1) {
      if(data[x] != '&') {
        field += data[x];
      }
      if(field.startsWith("ssid=")) {
        ssid = field.substring(5);
      } else if(field.startsWith("password=")) {
        pass = field.substring(9);
      }
      field = "";
    } else {
      field += data[x];
    }
  }

  if(ssid == "") {
    return false;
  }

  char ssidArr[32];
  char passwordArr[32];
  ssid.toCharArray(ssidArr, 32);
  pass.toCharArray(passwordArr, 32);

  ConfigManager *configManager = ConfigManager::getInstance();

  configManager->setSSID(ssidArr);
  configManager->setPassword(passwordArr);
  return configManager->save();
}