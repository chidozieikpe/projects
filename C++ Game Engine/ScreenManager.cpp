#include "Core.h"
#include "ScreenManager.h"

ScreenManager &ScreenManager::Instance() {
	static ScreenManager instance;
	return instance;
}

ScreenManager::ScreenManager() {

}

ScreenManager::~ScreenManager() {

}

void ScreenManager::initialize() {

}

void ScreenManager::load() {
	currentScreen->load();
}

void ScreenManager::draw(sf::RenderWindow & window) {
	currentScreen->draw(window);
}

void ScreenManager::update() {
	currentScreen->update();
}

void ScreenManager::setText(std::string text) {
	this->text = text;
}

void ScreenManager::drawText() {
	std::cout << text << std::endl;
}