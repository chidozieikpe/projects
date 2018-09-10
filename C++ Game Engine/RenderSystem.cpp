#include "Core.h"
#include "Map.h"
#include "RenderSystem.h"
#include "RenderableComponent.h"

void RenderSystem::load(XMLElement* element)
{
	XMLElement* xmlWindow = element->FirstChildElement("Window");
	if (xmlWindow != nullptr)
	{
		xmlWindow->QueryIntAttribute("width", &width);
		xmlWindow->QueryIntAttribute("height", &height);
		if (auto strTitle = xmlWindow->Attribute("title"))
		{
			title = strTitle;
		}
		xmlWindow->QueryIntAttribute("frameLimit", &frameLimit);
	}

	window = std::make_unique<sf::RenderWindow>(sf::VideoMode(width, height), title);
	window->setFramerateLimit(frameLimit);
	window->setKeyRepeatEnabled(false);		// we only want one event per type
}

void RenderSystem::save(XMLElement* element)
{

}

void RenderSystem::addRenderable(RenderableComponent* component)
{
	renderComponents.push_back(component);
}

void RenderSystem::removeRenderable(RenderableComponent* component)
{
	renderComponents.remove(component);
}

void RenderSystem::initialize()
{
	REGISTER_ABSTRACT_CLASS(RenderableComponent)
}




void RenderSystem::update(float _deltaTime)
{
	window->clear();
	//testing map loadening
	//Map::Instance().renderLevel(*window);

	for (auto& comp : renderComponents)
	{
		comp->render();
	}


	window->display();
}
