library( "InterfaceComponents/StatusBar" )

if playerHUDs == nil then
	playerHUDs = {}
end
hudColors = { Color.Red, Color.Blue, Color.Green, Color.Yellow } 

playerHUDs[playerHudIndex] = {}
playerHUDs[playerHudIndex].interface = Interface(GammaDraconis)
playerHUDs[playerHudIndex].interface.UpdateCall = "playerHUDs" .. playerHudIndex .. "update"

playerHUDs[playerHudIndex].lapText = Text(GammaDraconis)
playerHUDs[playerHudIndex].lapText.spriteFontName = "Resources/Fonts/Menu"
playerHUDs[playerHudIndex].lapText.color = Color.White
playerHUDs[playerHudIndex].lapText.RelativePosition = Vector2( 416, 16 )
playerHUDs[playerHudIndex].interface:AddComponent(playerHUDs[playerHudIndex].lapText)

playerHUDs[playerHudIndex].checkpointText = Text(GammaDraconis)
playerHUDs[playerHudIndex].checkpointText.spriteFontName = "Resources/Fonts/Menu"
playerHUDs[playerHudIndex].checkpointText.color = Color.White
playerHUDs[playerHudIndex].checkpointText.RelativePosition = Vector2( 512, 16 )
playerHUDs[playerHudIndex].interface:AddComponent(playerHUDs[playerHudIndex].checkpointText)

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
	local status = Engine.GetInstance().race:status(Player.players[playerIndex-1])
	playerHUDs[playerIndex].lapText.text = "Lap: " .. status.lap
	playerHUDs[playerIndex].checkpointText.text = "CP: " .. status.checkpoint
end

return playerHUDs[playerHudIndex].interface