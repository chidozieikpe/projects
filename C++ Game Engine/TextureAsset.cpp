#include "Core.h"
#include "TextureAsset.h"

IMPLEMENT_DYNAMIC_CLASS(TextureAsset)

TextureAsset::TextureAsset()
{
}

TextureAsset::~TextureAsset()
{
}

const sf::Texture& TextureAsset::getTexture() const
{
	return texture;
}

void TextureAsset::load(tinyxml2::XMLElement * _textureElement)
{
	Object::load(_textureElement);
	if (_textureElement != nullptr)
	{
		if (auto path = _textureElement->Attribute("path"))
		{
			bool result = texture.loadFromFile(path);
			ASSERT(result, "Cannot find texture, check the working directory setting!");
		}
	}
}
