#ifndef WEBCONFIG_H
#define WEBCONFIG_H

#include <ESP8266WiFi.h>

class WebConfig {
    private:
      WiFiServer server = WiFiServer(80);
      String prepareHtmlPage(String htmlContent);
      String prepareNetworksForm();
      bool decodeAndSaveData(String data);
    public:
      WebConfig();
      void listen();
};

#endif /* WEBCONFIG_H */