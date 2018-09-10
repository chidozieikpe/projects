#pragma once

#include "SpriteComponent.h"

// Modified By: Matt Warner, Implemented By Pranesh and Fernando
/// <summary>
/// AnimatedSprite class that works for SpriteSheets with same dimensions
///	SpriteAtlas should be used for non-uniform sprites
/// </summary>
/// <seealso cref="Component" />
/// <seealso cref="IRenderable" />
class AnimatedSpriteComponent : public SpriteComponent
{
	DECLARE_DYNAMIC_DERIVED_CLASS(AnimatedSpriteComponent, SpriteComponent)

	friend class RenderSystem;

public:
	AnimatedSpriteComponent();
	~AnimatedSpriteComponent();

	void update(float _deltaTime) override;
	void load(XMLElement *component) override;
	void save(tinyxml2::XMLElement *animatedSpriteRoot);

	//enum DIR{Down, Left, Right, Up};

	sf::Vector2i source;
	// elapsed time required to spitch to next sprite
	float spriteSwitchTime;
	float elapsedTime;
	bool moving;

private:
	
	std::map<int, sf::Vector2i> spriteIndexes;
	// holds the current sprite in the sprite map
	sf::Vector2i spriteIndex;


	

};