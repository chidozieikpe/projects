#pragma once

#include "Asset.h"
#include <string>
#include <SFML/Graphics.hpp>

// Implemented by Lucien and Regis Modified by Matt Warner
class TextureAsset : public Asset
{
	DECLARE_DYNAMIC_DERIVED_CLASS(TextureAsset, Asset)

public:

	TextureAsset();
	~TextureAsset();

	const sf::Texture& getTexture() const;
	void load(tinyxml2::XMLElement* _element) override;

private:
	std::string fileName;
	std::map<int, sf::Texture> textures;
	sf::Texture texture;
};

