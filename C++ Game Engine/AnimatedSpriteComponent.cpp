#include "Core.h"
#include "tinyxml2.h"
#include "AnimatedSpriteComponent.h"
#include "AssetManager.h"
//#include "Player.h"
#include "InputManager.h"

IMPLEMENT_DYNAMIC_CLASS(AnimatedSpriteComponent);

AnimatedSpriteComponent::AnimatedSpriteComponent()
	: spriteSwitchTime(0.0f),
	elapsedTime(0.0f),
	moving(false)
{
}

AnimatedSpriteComponent::~AnimatedSpriteComponent()
{
}

void AnimatedSpriteComponent::update(float _deltaTime)
{
	
	elapsedTime += _deltaTime;

	if (elapsedTime >= spriteSwitchTime)
	{
		// smoother animation
		elapsedTime -= spriteSwitchTime;
		
		if (moving) {
			source.x++;
		}
		else {
			source.x = 0;
		}
		if (source.x * 32 >= sprite.getTexture()->getSize().x) {
			source.x = 0;
		}
	}
	
	// sprite index has changed. update sprite rect
	//spriteRect.left = spriteIndexes[1].x * spriteRect.width;
	//// sprite index has changed. update sprite rect.
	//spriteRect.top = spriteIndexes[1].y * spriteRect.height;
	spriteRect = sf::IntRect(source.x * 32, source.y * 32, 32, 32);
}

void AnimatedSpriteComponent::load(XMLElement * component)
{
	SpriteComponent::load(component);
	component->QueryFloatAttribute("SwitchTime", &spriteSwitchTime);
	XMLElement* xmlSpriteIndex = component->FirstChildElement("SpriteIndex2");
	
	//Player::Instance();

	if (xmlSpriteIndex != nullptr)
	{
		xmlSpriteIndex->QueryIntAttribute("x", &spriteIndex.x);
		xmlSpriteIndex->QueryIntAttribute("y", &spriteIndex.y);
		spriteIndexes[1] = spriteIndex;
	}
	xmlSpriteIndex = component->FirstChildElement("SpriteIndex");
	if (xmlSpriteIndex != nullptr)
	{
		xmlSpriteIndex->QueryIntAttribute("x", &spriteIndex.x);
		xmlSpriteIndex->QueryIntAttribute("y", &spriteIndex.y);
		spriteIndexes[0] = spriteIndex;
	}

	source.x = 0;
	source.y = 0;
}

void AnimatedSpriteComponent::save(tinyxml2::XMLElement *animatedSpriteRoot)
{
	tinyxml2::XMLDocument doc;

	tinyxml2::XMLElement *animatedSprite = doc.NewElement("AnimatedSprite");
	animatedSprite->SetAttribute("spriteSwitchTime", spriteSwitchTime);
	animatedSprite->SetAttribute("x", spriteIndex.x);
	animatedSprite->SetAttribute("y", spriteIndex.y);

	animatedSpriteRoot->InsertFirstChild(animatedSprite);
}

