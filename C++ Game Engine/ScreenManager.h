#pragma once
#include <string>
#include<iostream>
#include "GameScreen.h"

class ScreenManager {
public:
	~ScreenManager();
	static ScreenManager &Instance();

	void initialize();
	void load();
	void update();
	void setText(std::string text);
	void draw(sf::RenderWindow & window);
	void drawText();

protected:

private:
	
	std::unique_ptr<GameScreen> newScreen, currentScreen;
	ScreenManager();
	ScreenManager(ScreenManager const&);
	void operator=(ScreenManager const&);

	std::string text;
};