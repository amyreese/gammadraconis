library( "InterfaceComponents/StatusBar" )

if playerHUDs == nil then
	playerHUDs = {}
end
hudColors = { Color.Red, Color.Blue, Color.Green, Color.Yellow } 

playerHUDs[playerHudIndex] = {}
playerHUDs[playerHudIndex].interface = Interface(GammaDraconis)
playerHUDs[playerHudIndex].interface.UpdateCall = "playerHUDs" .. playerHudIndex .. "update"

playerHUDs[playerHudIndex].testText = Text(GammaDraconis)
playerHUDs[playerHudIndex].testText.spriteFontName = "Resources/Fonts/Menu"
playerHUDs[playerHudIndex].testText.color = Color.White
playerHUDs[playerHudIndex].interface:AddComponent(playerHUDs[playerHudIndex].testText)

hudBorder = Sprite(GammaDraconis)
hudBorder.textureName = "Resources/Textures/HUD/HudBorder"
hudBorder.color = hudColors[playerHudIndex]
playerHUDs[playerHudIndex].interface:AddComponent(hudBorder)

hudBorder = Sprite(GammaDraconis)
hudBorder.textureName = "Resources/Textures/HUD/HudBorder"
hudBorder.color = hudColors[playerHudIndex]
hudBorder.RelativePosition = Vector2( 1024-256, 0 )
playerHUDs[playerHudIndex].interface:AddComponent(hudBorder)

hudBorder = Sprite(GammaDraconis)
hudBorder.textureName = "Resources/Textures/HUD/HudBorder"
hudBorder.color = hudColors[playerHudIndex]
hudBorder.RelativePosition = Vector2( 0, 768 - 8 )
playerHUDs[playerHudIndex].interface:AddComponent(hudBorder)

hudBorder = Sprite(GammaDraconis)
hudBorder.textureName = "Resources/Textures/HUD/HudBorder"
hudBorder.color = hudColors[playerHudIndex]
hudBorder.RelativePosition = Vector2( 1024-256, 768 - 8 )
playerHUDs[playerHudIndex].interface:AddComponent(hudBorder)

hudBorder = Sprite(GammaDraconis)
hudBorder.textureName = "Resources/Textures/HUD/HudBorder"
hudBorder.color = hudColors[playerHudIndex]
hudBorder.RelativePosition = Vector2( 8, 0 )
hudBorder.RelativeRotation = 3.14/2
playerHUDs[playerHudIndex].interface:AddComponent(hudBorder)

hudBorder = Sprite(GammaDraconis)
hudBorder.textureName = "Resources/Textures/HUD/HudBorder"
hudBorder.color = hudColors[playerHudIndex]
hudBorder.RelativePosition = Vector2( 1024, 0 )
hudBorder.RelativeRotation = 3.14/2
playerHUDs[playerHudIndex].interface:AddComponent(hudBorder)

hudBorder = Sprite(GammaDraconis)
hudBorder.textureName = "Resources/Textures/HUD/HudBorder"
hudBorder.color = hudColors[playerHudIndex]
hudBorder.RelativePosition = Vector2( 8, 768 - 256 )
hudBorder.RelativeRotation = 3.14/2
playerHUDs[playerHudIndex].interface:AddComponent(hudBorder)

hudBorder = Sprite(GammaDraconis)
hudBorder.textureName = "Resources/Textures/HUD/HudBorder"
hudBorder.color = hudColors[playerHudIndex]
hudBorder.RelativePosition = Vector2( 1024, 768 - 256 )
hudBorder.RelativeRotation = 3.14/2
playerHUDs[playerHudIndex].interface:AddComponent(hudBorder)

playerHUDs[playerHudIndex].statBar = StatusBar.new()
playerHUDs[playerHudIndex].statBar.addToInterface(playerHUDs[playerHudIndex].interface)
playerHUDs[playerHudIndex].statBar.relocate( Vector2( 512-64, 64 ) )

function playerHUDs1update(gameTime)
	playerHUDs.update(gameTime, 1)
end
function playerHUDs2update(gameTime)
	playerHUDs.update(gameTime, 2)
end
function playerHUDs3update(gameTime)
	playerHUDs.update(gameTime, 3)
end
function playerHUDs4update(gameTime)
	playerHUDs.update(gameTime, 4)
end
function playerHUDs.update(gameTime, playerIndex)
	playerHUDs[playerIndex].statBar.update(Player.players[playerIndex-1].velocity:pos():Length())
end

return playerHUDs[playerHudIndex].interface