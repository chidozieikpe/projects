#include "Core.h"
#include "SpriteComponent.h"
#include "RenderSystem.h"
#include "AssetManager.h"
#include "TextureAsset.h"
#include "GameObject.h"
#include "PrefabAsset.h"

IMPLEMENT_DYNAMIC_CLASS(SpriteComponent);

SpriteComponent::SpriteComponent()
	: spriteRect(0, 0, 0, 0),
		sprite(),
		spriteCount(0, 0),
		pivot(0, 0)
	
{
}

SpriteComponent::~SpriteComponent()
{
}

void SpriteComponent::setTexture(TextureAsset* texture)
{
	ASSERT(texture, "Sprite texture is null!");
	sprite.setTexture(texture->getTexture());
}

void SpriteComponent::setTextureRect(const sf::IntRect& rect)
{
	spriteRect = rect;
}

sf::IntRect SpriteComponent::getSpriteRect() {
	return spriteRect;
}

sf::Sprite SpriteComponent::getSprite() {
	return sprite;
}

void SpriteComponent::load(XMLElement * element)
{
	XMLElement* xmlSprite = element->FirstChildElement("Sprite");
	if (xmlSprite != nullptr)
	{
		if (auto textureName = xmlSprite->Attribute("texture"))
		{
			auto asset = AssetManager::Instance().GetAsset(textureName);
			if (asset != nullptr && asset->isA(TextureAsset::getClassHashCode()))
			{
				sprite.setTexture(std::static_pointer_cast<TextureAsset>(asset)->getTexture());
			}
			else {
				assert(0);
			}
		}
		xmlSprite->QueryIntAttribute("top", &spriteRect.top);
		xmlSprite->QueryIntAttribute("left", &spriteRect.left);
		xmlSprite->QueryIntAttribute("width", &spriteRect.width);
		xmlSprite->QueryIntAttribute("height", &spriteRect.height);
		XMLElement* xmlPivot = xmlSprite->FirstChildElement("Pivot");
		if (xmlPivot != nullptr)
		{
			xmlPivot->QueryFloatAttribute("x", &pivot.x);
			xmlPivot->QueryFloatAttribute("y", &pivot.y);
		}
		XMLElement* xmlSpriteCount = xmlSprite->FirstChildElement("SpriteCount");
		if (xmlSpriteCount != nullptr)
		{
			xmlSpriteCount->QueryIntAttribute("x", &spriteCount.x);
			xmlSpriteCount->QueryIntAttribute("y", &spriteCount.y);
			spriteNumbers[0] = spriteCount;
		}
		xmlSpriteCount = xmlSprite->FirstChildElement("SpriteCount2");
		if (xmlSpriteCount != nullptr)
		{
			xmlSpriteCount->QueryIntAttribute("x", &spriteCount.x);
			xmlSpriteCount->QueryIntAttribute("y", &spriteCount.y);
			spriteNumbers[1] = spriteCount;
		}
	}
}

void SpriteComponent::render()
{
	if (auto gObject = getOwner())
	{
		Transform* transform = gObject->getTransform();
		if (transform != nullptr)
		{
			sprite.setPosition(transform->getPosition());
			sprite.setRotation(transform->getLocalRotation());
			sprite.setScale(transform->getLocalScale());
			sprite.setTextureRect(spriteRect);
			RenderSystem::Instance().GetRenderWindow()->draw(sprite);
		}
	}
	else if (auto gObject = getOwnerPrefab()) {
		Transform* transform = gObject->getTransform();
		if (transform != nullptr)
		{
			sprite.setPosition(transform->getPosition());
			sprite.setRotation(transform->getLocalRotation());
			sprite.setScale(transform->getLocalScale());
			sprite.setTextureRect(spriteRect);
			RenderSystem::Instance().GetRenderWindow()->draw(sprite);
		}
	}
}
