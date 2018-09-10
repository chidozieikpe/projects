#pragma once

#include <list>
#include "ISystem.h"
#include "ISerializable.h"

#include <SFML/Graphics.hpp>
#include <SFML/OpenGL.hpp>

class RenderableComponent;

// Implemented by Lucien and Regis
/// <summary>
/// Render System Instance
/// </summary>
/// <seealso cref="ISystem" />
class RenderSystem : public ISystem, public ISerializable
{
private:
	std::list<RenderableComponent*> renderComponents;
	int width;
	int height;
	std::string title;
	int frameLimit;
	std::unique_ptr<sf::RenderWindow> window;
public:
	RenderSystem(const RenderSystem&) = delete;
	RenderSystem& operator=(const RenderSystem&) = delete;

	/// <summary>
	/// Singleton reference to this instance.
	/// </summary>
	/// <returns></returns>
	inline static RenderSystem& Instance()
	{
		static RenderSystem instance;
		return instance;
	}

	void load(XMLElement* element) override;
	void save(XMLElement* element) override;

	/// <summary>
	/// Adds the renderable component
	/// </summary>
	/// <param name="component">The component.</param>
	void addRenderable(RenderableComponent* component);

	/// <summary>
	/// Removes the renderable component
	/// </summary>
	/// <param name="component">The component.</param>
	void removeRenderable(RenderableComponent* component);


	sf::RenderWindow* GetRenderWindow() { return window.get(); }

protected:
	/// <summary>
	/// Initializes this instance.
	/// </summary>
	void initialize() override;

	/// <summary>
	/// Updates this instance.
	/// </summary>
	void update(float _deltaTime) override;

private:
	RenderSystem() = default;
	~RenderSystem() = default;


	friend class GameEngine;

};

