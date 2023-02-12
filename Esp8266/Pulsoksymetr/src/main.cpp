#include <Arduino.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>
#include <ESP8266HTTPClient.h>
#include <WiFiClient.h>
#include "ConfigManager.h"
#include "WebConfig.h"

#include <Wire.h>
#include "LittleFS.h"
#include <ArduinoJson.h>
#include <WiFiClientSecureBearSSL.h>

#include "DFRobot_BloodOxygen_S.h"
#define I2C_COMMUNICATION  //use I2C for communication, but use the serial port for communication if the line of codes were masked

#ifdef  I2C_COMMUNICATION
#define I2C_ADDRESS    0x57
  DFRobot_BloodOxygen_S_I2C MAX30102(&Wire ,I2C_ADDRESS);
#else
/* ---------------------------------------------------------------------------------------------------------------
 *    board   |             MCU                | Leonardo/Mega2560/M0 |    UNO    | ESP8266 | ESP32 |  microbit  |
 *     VCC    |            3.3V/5V             |        VCC           |    VCC    |   VCC   |  VCC  |     X      |
 *     GND    |              GND               |        GND           |    GND    |   GND   |  GND  |     X      |
 *     RX     |              TX                |     Serial1 TX1      |     5     |   5/D6  |  D2   |     X      |
 *     TX     |              RX                |     Serial1 RX1      |     4     |   4/D7  |  D3   |     X      |
 * ---------------------------------------------------------------------------------------------------------------*/
#if defined(ARDUINO_AVR_UNO) || defined(ESP8266)
SoftwareSerial mySerial(4, 5);
DFRobot_BloodOxygen_S_SoftWareUart MAX30102(&mySerial, 9600);
#else
DFRobot_BloodOxygen_S_HardWareUart MAX30102(&Serial1, 9600); 
#endif
#endif

ConfigManager *configManager = ConfigManager::getInstance();
//#include <WiFiClientSecureBearSSL.h>

//ESP8266WiFiMulti WiFiMulti;
WebConfig webConfig;



WiFiServer server(80);
#define USE_SERIAL Serial
#define SCREEN_WIDTH 128 // OLED display width, in pixels
#define SCREEN_HEIGHT 64 // OLED display height, in pixels

#define OLED_RESET     -1 // Reset pin # (or -1 if sharing Arduino reset pin)
#define SCREEN_ADDRESS 0x3C ///< See datasheet for Address; 0x3D for 128x64, 0x3C for 128x32
Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, OLED_RESET);

const int EEPROM_SIZE = 1024; // The ESP8266 has 1024 bytes of EEPROM
const int ADDRESS = 0;

bool newMeasurement = true;
char ssid[64] = "", pass[64] = "";
const char* deviceId = "";
StaticJsonDocument<179> deviceResponseDoc;
String macAddress = "";
bool isWiFiWanted = true;
//std::unique_ptr<BearSSL::WiFiClientSecure>client(new BearSSL::WiFiClientSecure);




int averageHeartRate = 0;
int maximumHeartRate = 0;
int minimumHeartRate = 1000;
int averageSpO2 = 0;
int maximumSpO2 = 0;
int minimumSpO2 = 1000;
int numberOfMeasurements = 0;
const char* wifiId = "";
/*#define NEW_RESULTS 1
#define OLD_RESULTS 2
#define DEVICE 3
#define WIFI 4*/
enum class Types
{
  newResults = 1,
  oldResults = 2,
  device = 3,
  wifi = 4
};
    std::unique_ptr<BearSSL::WiFiClientSecure>client(new BearSSL::WiFiClientSecure);
    
   

StaticJsonDocument<179> wifiResponseDoc;

void displayOnScreen(String row1, String row2 = "", String row3 = "", String row4 = "", String row5 = "", String row6 = "", String row7 = "")
{
  display.clearDisplay();
  display.setCursor(1,1);
  display.write(row1.c_str());
  display.setCursor(1,10);
  display.write(row2.c_str());
  display.setCursor(1,19);
  display.write(row3.c_str());
  display.setCursor(1,28);
  display.write(row4.c_str());
  display.setCursor(1,37);
  display.write(row5.c_str());
  display.setCursor(1,46);
  display.write(row6.c_str());
  display.setCursor(1,55);
  display.write(row7.c_str());
  display.display();
}

/*void sendDataToServer(DynamicJsonDocument doc, String apiUrl, Types type)
{
  std::unique_ptr<BearSSL::WiFiClientSecure>client(new BearSSL::WiFiClientSecure);

  client->setInsecure();

  HTTPClient https;
  if (https.begin(*client, apiUrl)) {  // HTTPS
    https.addHeader("Content-Type", "application/json");

    String dto = "";
    serializeJson(doc, dto);

    int httpCode = https.POST(dto);

    if (httpCode > 0) 
    {
      if (httpCode == HTTP_CODE_OK) 
      {
        String payload;
        DeserializationError error;
        switch (type)
        {
          case Types::newResults:
            displayOnScreen("Measurement added", "Put finger on sensor", "for another measurement");
            break;
          case Types::oldResults:
            displayOnScreen("Measurements from memory", "has been added");
            break;
          case Types::device:
            payload = http.getString();
            error = deserializeJson(deviceResponseDoc, payload);

            if (error) {
              Serial.print(F("deserializeJson() failed: "));
              Serial.println(error.f_str());
            }
            Serial.print(payload);
            deviceId = deviceResponseDoc["id"];
            break;

          case Types::wifi:
            payload = http.getString();
            error = deserializeJson(wifiResponseDoc, payload);

            if (error) {
              Serial.print(F("deserializeJson() failed: "));
              Serial.println(error.f_str());
            }

            wifiId = wifiResponseDoc["id"];
            break;
        }

      }
    } 
    else
    {
      USE_SERIAL.printf("[HTTP] POST... failed, error: %s\n", http.errorToString(httpCode).c_str());
    }
  }
  https.end();
}*/

void setup() 
{
  Serial.begin(115200);
 WiFi.disconnect(true);
  // SSD1306_SWITCHCAPVCC = generate display voltage from 3.3V internally
  if(!display.begin(SSD1306_SWITCHCAPVCC, SCREEN_ADDRESS)) {
    Serial.println(F("SSD1306 allocation failed"));
    for(;;); // Don't proceed, loop forever
  }

  display.clearDisplay();
  display.setCursor(1,1);
  display.setTextColor(SSD1306_WHITE);
  display.setTextSize(1);
  display.display();



  while (false == MAX30102.begin())
  {
    displayOnScreen("MAX30102 not found", "Check wiring/power");
    
    delay(500);
  }
  MAX30102.sensorStartCollect();

  macAddress = WiFi.macAddress();
  configManager->setSSID("Super Internet");
  configManager->setPassword("jwlo8485");
  configManager->save();

  if(!configManager->load()) 
  {
    displayOnScreen("Wifi configuration", "wasn't loaded");
    delay(1500);
  }
  else
  {
    displayOnScreen("Wifi configuration", "is loaded");
    configManager->getSSID(ssid);
    configManager->getPassword(pass);
    delay(1500);
  }
Serial.println(WiFi.status());
  while(WiFi.status() != WL_CONNECTED && isWiFiWanted)
  {
    Serial.println("TUTAJ JESTEM 1");
    delay(10);
    if(!configManager->isDeviceConfigured()) 
    {
      WiFi.mode(WIFI_STA);
      WiFi.disconnect();
      delay(100);
      Serial.println("TUTAJ JESTEM 2");
      IPAddress localIp(192,168,1,1);
      IPAddress gateway(192,168,1,1);
      IPAddress subnet(255,255,255,0);
      WiFi.softAPConfig(localIp, gateway, subnet);
      WiFi.softAP("PulseoximeterEsp8266");
      displayOnScreen("Connect with wifi", "PulseoximeterEsp8266,", "go to 192.168.1.1", "and enter your", "wifi credentials.", "Put finger on sensor", "to cancel connecting");
      while(!configManager->isDeviceConfigured()) {// tutaj można zrobić check z przyłożeniem palca do sensora :o jeśli się nie chce wifi done i guess 
        webConfig.listen();
        MAX30102.getHeartbeatSPO2();
        Serial.println("TUTAJ JESTEM 3");
        Serial.println(MAX30102._sHeartbeatSPO2.Heartbeat);
        if(MAX30102._sHeartbeatSPO2.Heartbeat != -1)
        {
          displayOnScreen("No WiFi", "Put finger on sensor", "Heartrate: ---", "SpO2: ---");
          Serial.println("TUTAJ JESTEM 3.5");
          isWiFiWanted = false;
          break;
        }      
      }

      if(isWiFiWanted == false)
        break;
    }
 
    if(configManager->isDeviceConfigured())
    {
      Serial.println("TUTAJ JESTEM 4");
      configManager->getSSID(ssid);
      configManager->getPassword(pass);
      WiFi.begin(ssid, pass);
      WiFi.mode(WIFI_STA);
      //WiFiMulti.addAP(ssid, pass);
      Serial.print(ssid);
      Serial.print(pass);
      displayOnScreen("Connecting with ", ssid);
      display.setCursor(1,19);
      display.display();
      while (WiFi.status() != WL_CONNECTED) 
      {
        delay(500);
        display.write(".");
        display.display();
        Serial.println("TUTAJ JESTEM 5");
        
        if(WiFi.status() == WL_WRONG_PASSWORD || WiFi.status() == WL_NO_SSID_AVAIL)
        {
          displayOnScreen("Wrong password or ssid");
          configManager->setPassword("");
          configManager->setSSID("");
          delay(1500);
          break;
        }
      }
    }
  }

  if(WiFi.status() == WL_CONNECTED)
  {
    displayOnScreen("Connected");
    delay(1500);
    displayOnScreen(ssid, "Put finger on sensor", "Heartrate: ---", "SpO2: ---");
  }

  
  

  delay(100);
}



///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void loop() 
{
  MAX30102.getHeartbeatSPO2();
  delay(10);

  if (WiFi.status() == WL_CONNECTED && newMeasurement == true) // i jakiś and new pomiar
  {
    HTTPClient https;
    client->setInsecure();
    if (https.begin(*client, "https://192.168.195.34:7212/api/Wifis")) 
    {  // HTTPS

      Serial.print("[HTTPS] GET...\n");
      // start connection and send HTTP header
      https.addHeader("Content-Type", "application/json");
      DynamicJsonDocument wifiDoc(200);
      wifiDoc["Name"] = ssid;
      wifiDoc["Password"] = pass;

      String dto = "";

      serializeJson(wifiDoc, dto);
      delay(10);
      int httpCode = https.POST(dto);
      delay(10);

      if (httpCode > 0) {
        Serial.printf("[HTTPS] POST... code: %d\n", httpCode);

        if (httpCode == HTTP_CODE_OK || httpCode == HTTP_CODE_MOVED_PERMANENTLY) {

          String payload;
          DeserializationError error;

          payload = https.getString();
          Serial.println(payload);
            error = deserializeJson(wifiResponseDoc, payload);

            if (error) {
              Serial.print(F("deserializeJson() failed: "));
              Serial.println(error.f_str());
            }

            wifiId = wifiResponseDoc["id"];
        }
      } else {
        Serial.printf("[HTTPS] POST... failed, error: %s\n", https.errorToString(httpCode).c_str());
      }

      https.end();
    } 
    else 
    {
      Serial.printf("[HTTPS] Unable to connect\n");
    }

    HTTPClient https2;
    //client->setInsecure();

    if (https2.begin(*client, "https://192.168.195.34:7212/api/Devices")) 
    {  // HTTPS

      Serial.print("[HTTPS] GET...\n");
      // start connection and send HTTP header
      https2.addHeader("Content-Type", "application/json");
      DynamicJsonDocument deviceDoc(160);
      deviceDoc["MacAddress"] = macAddress;

      String dto = "";

      serializeJson(deviceDoc, dto);
      Serial.println(dto);
      delay(10);
      int httpCode = https2.POST(dto);
      delay(10);
      Serial.printf("[HTTPS] POST... code: %d\n", httpCode);
      if (httpCode > 0) 
      {
        Serial.printf("[HTTPS] POST... code: %d\n", httpCode);

        if (httpCode == HTTP_CODE_OK || httpCode == HTTP_CODE_MOVED_PERMANENTLY) {

          String payload;
          DeserializationError error;

          payload = https2.getString();
            error = deserializeJson(deviceResponseDoc, payload);

            if (error) {
              Serial.print(F("deserializeJson() failed: "));
              Serial.println(error.f_str());
            }

            deviceId = deviceResponseDoc["id"];
        }
      } 
      else 
      {
        Serial.printf("[HTTPS] POST... failed, error: %s\n", https2.errorToString(httpCode).c_str());
      }

      https2.end();
    } 
    else 
    {
      Serial.printf("[HTTPS] Unable to connect\n");
    }

    //file check

    LittleFS.begin();
    if(LittleFS.exists("/data.json"))
    {
      Serial.println("Istnieje");
      File f = LittleFS.open("/data.json","r");
      bool IsAdded = false;
      while(f.available())
      {
        DynamicJsonDocument oldResultsDoc(384);
        deserializeJson(oldResultsDoc, f);

        if (https.begin(*client, "https://192.168.195.34:7212/api/Measurements")) 
        {  // HTTPS

          Serial.print("[HTTPS] GET...\n");
          // start connection and send HTTP header
          https.addHeader("Content-Type", "application/json");
          String dto = "";

          oldResultsDoc["wifiId"] = wifiId;
          oldResultsDoc["deviceId"] = deviceId;

          serializeJson(oldResultsDoc, dto);
          Serial.print(dto);

          delay(10);
          int httpCode = https.POST(dto);
          delay(10);

          if (httpCode > 0) 
          {
            Serial.printf("[HTTPS] POST... code: %d\n", httpCode);

            if (httpCode == HTTP_CODE_OK || httpCode == HTTP_CODE_MOVED_PERMANENTLY) 
            {
              displayOnScreen(ssid, "Results from memory", "has been added");
              delay(1500);
              IsAdded = true;
            }
            else 
            {
              Serial.printf("[HTTPS] POST... failed, error: %s\n", https.errorToString(httpCode).c_str());
            }

            https.end();
          } 
          else 
          {
            Serial.printf("[HTTPS] Unable to connect\n");
          }
        }
        oldResultsDoc.clear();
      }
      f.close();
      if(IsAdded)
        LittleFS.remove("/data.json");
    }
    LittleFS.end();

  }

  MAX30102.getHeartbeatSPO2();
  Serial.print("Heartbeat: ");
  Serial.println(MAX30102._sHeartbeatSPO2.Heartbeat);
  Serial.print("Spo2: ");
  Serial.println(MAX30102._sHeartbeatSPO2.SPO2);
  if (MAX30102._sHeartbeatSPO2.Heartbeat != -1 && newMeasurement)
  {
    if(WiFi.status() == WL_CONNECTED)
      displayOnScreen(ssid, "Heartrate: ---", "SpO2: ---");
    else
      displayOnScreen("No WiFi", "Heartrate: ---", "SpO2: ---");
    Serial.println("nowy pomiar");
    newMeasurement = false;
  }


  delay(1000);

    //MAKING AVERAGE, MAX and MIN
  MAX30102.getHeartbeatSPO2();
  if(MAX30102._sHeartbeatSPO2.Heartbeat != -1 && MAX30102._sHeartbeatSPO2.SPO2 != -1)
  {
    numberOfMeasurements++;
    averageHeartRate = (averageHeartRate * (numberOfMeasurements-1) + MAX30102._sHeartbeatSPO2.Heartbeat)/numberOfMeasurements;
    if(maximumHeartRate<MAX30102._sHeartbeatSPO2.Heartbeat)
      maximumHeartRate = MAX30102._sHeartbeatSPO2.Heartbeat;
    else if(minimumHeartRate>MAX30102._sHeartbeatSPO2.Heartbeat)
      minimumHeartRate = MAX30102._sHeartbeatSPO2.Heartbeat;

    averageSpO2 = (averageSpO2 * (numberOfMeasurements-1) + MAX30102._sHeartbeatSPO2.SPO2)/numberOfMeasurements;
    if(maximumSpO2<MAX30102._sHeartbeatSPO2.SPO2)
      maximumSpO2 = MAX30102._sHeartbeatSPO2.SPO2;
    else if(minimumSpO2>MAX30102._sHeartbeatSPO2.SPO2)
      minimumSpO2 = MAX30102._sHeartbeatSPO2.SPO2;
    if(WiFi.status() == WL_CONNECTED)
      displayOnScreen(ssid, "Heartrate: " + String(MAX30102._sHeartbeatSPO2.Heartbeat) + " bpm", "SpO2: " + String(MAX30102._sHeartbeatSPO2.SPO2) + "%");
    else
      displayOnScreen("No WiFi", "Heartrate: " + String(MAX30102._sHeartbeatSPO2.Heartbeat) + " bpm", "SpO2: " + String(MAX30102._sHeartbeatSPO2.SPO2) + "%");
  }



  if (newMeasurement == false && MAX30102._sHeartbeatSPO2.Heartbeat ==-1 && WiFi.status() == WL_CONNECTED && minimumHeartRate != 1000) 
  {
    HTTPClient https;
    client->setInsecure();



    Serial.print("[HTTPS] begin...\n");
    if (https.begin(*client, "https://192.168.195.34:7212/api/Measurements")) {  // HTTPS

      Serial.print("[HTTPS] GET...\n");
      // start connection and send HTTP header
      https.addHeader("Content-Type", "application/json");
      DynamicJsonDocument doc(384);
      doc["averageHeartRate"] = averageHeartRate;
      doc["maximumHeartRate"] = maximumHeartRate;
      doc["minimumHeartRate"] = minimumHeartRate;
      doc["averageSpO2"] = averageSpO2;
      doc["maximumSpO2"] = maximumSpO2;
      doc["minimumSpO2"] = minimumSpO2;
      doc["wifiId"] = wifiId;
      doc["deviceId"] = deviceId;

      String dto = "";

      serializeJson(doc, dto);
      Serial.print(dto);
      int httpCode = https.POST(dto);
      delay(10);

      if (httpCode > 0) 
      {
        Serial.printf("[HTTPS] POST... code: %d\n", httpCode);

        if (httpCode == HTTP_CODE_OK || httpCode == HTTP_CODE_MOVED_PERMANENTLY) 
        {
          displayOnScreen(ssid, "Results added", "Put finger on sensor", "Heartrate: ---", "SpO2: ---");
          delay(1500);
          averageHeartRate = 0;
          maximumHeartRate = 0;
          minimumHeartRate = 1000;
          averageSpO2 = 0;
          maximumSpO2 = 0;
          minimumSpO2 = 1000;
          numberOfMeasurements = 0;
          newMeasurement = true;
        }
      } 
      else 
      {
        Serial.printf("[HTTPS] POST... failed, error: %s\n", https.errorToString(httpCode).c_str());
      }

      https.end();
    } 
    else 
    {
      Serial.printf("[HTTPS] Unable to connect\n");
    }


  }
  if (newMeasurement == false && MAX30102._sHeartbeatSPO2.Heartbeat == -1 && minimumHeartRate != 1000)
  {
    DynamicJsonDocument doc(384);
    doc["averageHeartRate"] = averageHeartRate;
    doc["maximumHeartRate"] = maximumHeartRate;
    doc["minimumHeartRate"] = minimumHeartRate;
    doc["averageSpO2"] = averageSpO2;
    doc["maximumSpO2"] = maximumSpO2;
    doc["minimumSpO2"] = minimumSpO2;
    doc["wifiId"] = wifiId;
    doc["deviceId"] = deviceId;

    LittleFS.begin();
    File file = LittleFS.open("/data.json", "a");
    if (!file) 
    {
      Serial.println("Failed to open file for writing");
    }
    serializeJson(doc, file);
    file.println(); //add new line
    file.close();
    
    averageHeartRate = 0;
    maximumHeartRate = 0;
    minimumHeartRate = 1000;
    averageSpO2 = 0;
    maximumSpO2 = 0;
    minimumSpO2 = 1000;
    numberOfMeasurements = 0;
    newMeasurement = true;
    isWiFiWanted = true;
    LittleFS.end();

    if(WiFi.status() == WL_CONNECTED)
    {
      displayOnScreen(ssid, "Results saved", "in memory", "Put finger on sensor", "Heartrate: ---", "SpO2: ---");
      delay(1500);
    }
    else
    {
      displayOnScreen("No WiFi", "Results saved", "in memory", "Put finger on sensor", "Heartrate: ---", "SpO2: ---");
      delay(1500);
    }
    
    while(WiFi.status() != WL_CONNECTED && isWiFiWanted)
    {
    Serial.println("TUTAJ JESTEM 1");
    delay(10);
    if(!configManager->isDeviceConfigured()) 
    {
      WiFi.mode(WIFI_STA);
      WiFi.disconnect();
      delay(100);
      Serial.println("TUTAJ JESTEM 2");
      IPAddress localIp(192,168,1,1);
      IPAddress gateway(192,168,1,1);
      IPAddress subnet(255,255,255,0);
      WiFi.softAPConfig(localIp, gateway, subnet);
      WiFi.softAP("PulseoximeterEsp8266");
      displayOnScreen("Connect with wifi", "PulseoximeterEsp8266,", "go to 192.168.1.1", "and enter your", "wifi credentials.", "Put finger on sensor", "to cancel connecting");
      while(!configManager->isDeviceConfigured()) {// tutaj można zrobić check z przyłożeniem palca do sensora :o jeśli się nie chce wifi done i guess 
        webConfig.listen();
        MAX30102.getHeartbeatSPO2();
        if(MAX30102._sHeartbeatSPO2.Heartbeat != -1)
        {
          displayOnScreen("No WiFi", "Put finger on sensor", "Heartrate: ---", "SpO2: ---");
          Serial.println("TUTAJ JESTEM 3");
          isWiFiWanted = false;
          break;
        }      
      }

      if(isWiFiWanted == false)
        break;
    }
 
    if(configManager->isDeviceConfigured())
    {
      Serial.println("TUTAJ JESTEM 4");
      configManager->getSSID(ssid);
      configManager->getPassword(pass);
      WiFi.begin(ssid, pass);
      WiFi.mode(WIFI_STA);
      //WiFiMulti.addAP(ssid, pass);
      Serial.print(ssid);
      Serial.print(pass);
      displayOnScreen("Connecting with ", ssid);
      display.setCursor(1,19);
      display.display();
      while (WiFi.status() != WL_CONNECTED) 
      {
        delay(500);
        display.write(".");
        display.display();
        Serial.println("TUTAJ JESTEM 5");
        
        if(WiFi.status() == WL_WRONG_PASSWORD || WiFi.status() == WL_NO_SSID_AVAIL)
        {
          displayOnScreen("Wrong password or ssid");
          configManager->setPassword("");
          configManager->setSSID("");
          delay(1500);
          Serial.println("TUTAJ JESTEM 6");
          break;
        }
      }
    }
  }

    
  }

}


