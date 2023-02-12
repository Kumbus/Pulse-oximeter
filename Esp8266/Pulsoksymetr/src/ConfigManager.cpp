#include "ConfigManager.h"
#include <ArduinoJson.h>
#include "LittleFS.h"

ConfigManager* ConfigManager::configManager = nullptr;

ConfigManager::ConfigManager() {
  
  this->ssid = new char[64];
  this->pass = new char[64];
}

ConfigManager::~ConfigManager() {
  delete[] this->ssid;
  delete[] this->pass;
}

ConfigManager* ConfigManager::getInstance() {
  if(configManager==nullptr)
    configManager = new ConfigManager();

  return configManager;
}

bool ConfigManager::load() {
  const char* ssid2;
  const char* password2;
  LittleFS.begin();
  

  File f = LittleFS.open("/configuration.json", "r");
  if (f) 
  {

    String s = f.readString();
    f.close();
    StaticJsonDocument<179> doc;
    DeserializationError error = deserializeJson(doc, s);

    ssid2 = doc["ssid"]; 
    password2 = doc["pass"]; 

  }
  LittleFS.end();

  strcpy(this->ssid, ssid2);

  strcpy(this->pass, password2);
  
  LittleFS.end();

  return true;
}

bool ConfigManager::save() {
  LittleFS.begin();
  StaticJsonDocument<200> jsonDocument;

  jsonDocument["ssid"] = this->ssid;
  jsonDocument["pass"] = this->pass;
  
  File configFile = LittleFS.open("/configuration.json", "w");
  if (!configFile) {
    LittleFS.end();
    return false;
  }

  serializeJson(jsonDocument, configFile);
  configFile.close();
  LittleFS.end();
  return true;
  
}

bool ConfigManager::isDeviceConfigured() {
  return this->ssid[0] != '\0';
}

void ConfigManager::getSSID(char *buf) {
  strcpy(buf, this->ssid);

}

void ConfigManager::setSSID(const char *ssid) {
  strcpy(this->ssid, ssid);

}

void ConfigManager::getPassword(char *buf) {
  strcpy(buf, this->pass);

}

void ConfigManager::setPassword(const char *password) {
  strcpy(this->pass, password);

}