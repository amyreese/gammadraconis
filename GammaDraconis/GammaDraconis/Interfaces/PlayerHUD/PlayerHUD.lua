library( "InterfaceComponents/StatusBar" )
library( "InterfaceComponents/PositionArrow" )

if playerHUDs == nil then
	playerHUDs = {}
end
hudColors = { Color.Red, Color.Blue, Color.Green, Color.Yellow } 

playerHUDs[playerHudIndex] = {}
playerHUDs[playerHudIndex].interface = Interface(GammaDraconis)
playerHUDs[playerHudIndex].interface.UpdateCall = "playerHUDs" .. playerHudIndex .. "update"

playerHUDs[playerHudIndex].statusText = Text(GammaDraconis)
playerHUDs[playerHudIndex].statusText.spriteFontName = "Resources/Fonts/Menu"
playerHUDs[playerHudIndex].statusText.color = Color.White
playerHUDs[playerHudIndex].statusText.center = true
playerHUDs[playerHudIndex].statusText.RelativePosition = Vector2( 512, 16 )
playerHUDs[playerHudIndex].interface:AddComponent(playerHUDs[playerHudIndex].statusText)

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

reticule = Sprite(GammaDraconis, Rectangle(0,64,64,64))
reticule.textureName = "Resources/Textures/HUD/Elements"
reticule.RelativePosition = Vector2( 512-32, 384-32 )
reticule.RelativeRotation = 0
playerHUDs[playerHudIndex].interface:AddComponent(reticule)

target = Sprite(GammaDraconis, Rectangle(0,0,64,64))
target.textureName = "Resources/Textures/HUD/Elements"
target.RelativePosition = Vector2( 512-32, 384-32 )
target.RelativeRotation = 0
playerHUDs[playerHudIndex].interface:AddComponent(target)

hudMap = Sprite(GammaDraconis, Rectangle( 320, 0, 192, 128 ))
hudMap.textureName = "Resources/Textures/HUD/Elements"
hudMap.RelativePosition = Vector2( 800, 600)
hudMap.RelativeRotation = 0
playerHUDs[playerHudIndex].interface:AddComponent(hudMap)

hudMapPositions = {}
hudMapPositions[0] = Vector2( 870, 700 )
hudMapPositions[1] = Vector2( 950, 650 )
hudMapPositions[2] = Vector2( 870, 580 )
hudMapPositions[3] = Vector2( 800, 640 )

playerHUDs[playerHudIndex].arrows = {}
for x = 1,4 do
	playerHUDs[playerHudIndex].arrows[x] = PositionArrow.new(x)
	playerHUDs[playerHudIndex].arrows[x].addToInterface(playerHUDs[playerHudIndex].interface)
	playerHUDs[playerHudIndex].arrows[x].relocate( Vector2( 800, 700 ) )
end

--TODO:Percent finsihed indicator in bar format
--TODO:HUD indicator showing other users off screen (maybe on screen?)
--TODO:Arrow position showing place/relative location of other players


playerHUDs[playerHudIndex].speedBar = StatusBar.new()
playerHUDs[playerHudIndex].speedBar.addToInterface(playerHUDs[playerHudIndex].interface)
playerHUDs[playerHudIndex].speedBar.relocate( Vector2( 512-64, 128 ) )
playerHUDs[playerHudIndex].speedBar.color( Color.Gray )

playerHUDs[playerHudIndex].healthBar = StatusBar.new()
playerHUDs[playerHudIndex].healthBar.addToInterface(playerHUDs[playerHudIndex].interface)
playerHUDs[playerHudIndex].healthBar.relocate( Vector2( 512-64, 64 ) )
playerHUDs[playerHudIndex].healthBar.color( Color.Red )

playerHUDs[playerHudIndex].shieldBar = StatusBar.new()
playerHUDs[playerHudIndex].shieldBar.addToInterface(playerHUDs[playerHudIndex].interface)
playerHUDs[playerHudIndex].shieldBar.relocate( Vector2( 512-64, 96 ) )
playerHUDs[playerHudIndex].shieldBar.color( Color.Blue )

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
	local status = Engine.GetInstance().race:status(Player.players[playerIndex-1])
	local checkpoints = Engine.GetInstance().race.course.path.Count
	playerHUDs[playerIndex].healthBar.update(Player.players[playerIndex-1].health / Player.players[playerIndex-1].maxHealth)
	playerHUDs[playerIndex].shieldBar.update(Player.players[playerIndex-1].shield / Player.players[playerIndex-1].maxShield)
	playerHUDs[playerIndex].speedBar.update(Player.players[playerIndex-1].velocity:pos():Length() / 4.1)
	if status.place == 0 then
		playerHUDs[playerIndex].statusText.text = "Lap: " .. status.lap .. "  CP: " .. status.checkpoint .. "  Leading: " .. status.leading .. "  Following: " .. status.following
	else
		if status.place == 1 then
			playerHUDs[playerIndex].statusText.text = "Congratulations! You won!"
		else
			playerHUDs[playerIndex].statusText.text = "Place: " .. status.place 
		end
	end
	
end

return playerHUDs[playerHudIndex].interface