#pragma once

#include "RenderableComponent.h"
#include <SFML/Graphics.hpp>
#include <SFML/OpenGL.hpp>

class TextureAsset;

// Bug fixes and Modified by Matt Warner, Implemented by Lucien and Regis
/// <summary>
/// Sprite
/// </summary>
/// <seealso cref="Component" />
/// <seealso cref="IRenderable" />
class SpriteComponent : public RenderableComponent
{
	DECLARE_DYNAMIC_DERIVED_CLASS(SpriteComponent, RenderableComponent)

	friend class RenderSystem;

public:
	SpriteComponent();
	~SpriteComponent();

	void setTexture(TextureAsset* texture);
	void setTextureRect(const sf::IntRect& rect);

	void load(XMLElement* element) override;

	sf::IntRect getSpriteRect();
	sf::Sprite getSprite();

protected:
	/// <summary>
	/// Renders sprite.
	/// </summary>
	void render() override;

	sf::Sprite sprite;
	sf::IntRect spriteRect;
	sf::Vector2f pivot;
	// number of sprites in a sprite sheet (x,y)
	sf::Vector2i spriteCount;
	//sprite count map
	std::map<int, sf::Vector2i> spriteNumbers;
};

