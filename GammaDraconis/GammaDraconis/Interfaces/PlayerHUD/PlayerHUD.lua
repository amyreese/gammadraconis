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

--[[hudMap = Sprite(GammaDraconis, Rectangle( 320, 0, 192, 128 ))
hudMap.textureName = "Resources/Textures/HUD/Elements"
hudMap.RelativePosition = Vector2( 800, 600)
hudMap.RelativeRotation = 0
playerHUDs[playerHudIndex].interface:AddComponent(hudMap)

hudMapPositions = {}
hudMapPositions[0] = Vector2( 870, 700 )
hudMapPositions[1] = Vector2( 950, 650 )
hudMapPositions[2] = Vector2( 870, 580 )
hudMapPositions[3] = Vector2( 800, 640 )
]]--

playerHUDs[playerHudIndex].finishBars = {}
playerHUDs[playerHudIndex].finishIcons = {}
playerHUDs[playerHudIndex].placeIcons = {}
for x = 1,4 do
	--Initialize Finish Icons Array
	playerHUDs[playerHudIndex].finishIcons[x] = PositionArrow.new(x)
	playerHUDs[playerHudIndex].finishIcons[x].addToInterface(playerHUDs[playerHudIndex].interface)
	playerHUDs[playerHudIndex].finishIcons[x].relocate( Vector2( 750, 595 + (x * 30) ) )
	
	--Initialize status bars to track player progress
	playerHUDs[playerHudIndex].finishBars[x] = StatusBar.new()
	playerHUDs[playerHudIndex].finishBars[x].addToInterface(playerHUDs[playerHudIndex].interface)
	playerHUDs[playerHudIndex].finishBars[x].relocate( Vector2( 800, 600 + (x * 30) ) )
	playerHUDs[playerHudIndex].finishBars[x].color( Color.Yellow )
	
	--Initialize icons to determine what place the player is in
	playerHUDs[playerHudIndex].placeIcons[x] = PositionArrow.new(x)
	playerHUDs[playerHudIndex].placeIcons[x].addToInterface(playerHUDs[playerHudIndex].interface)
	playerHUDs[playerHudIndex].placeIcons[x].relocate( Vector2( 10 * x, 300 ) )
	playerHUDs[playerHudIndex].placeIcons[x].rescale( Vector2( .75, .75))
	playerHUDs[playerHudIndex].placeIcons[x].rotate( -MathHelper.PiOver2 )
end

--playerHUDs[playerHudIndex].placeIcons[playerHudIndex].rescale( Vector2( 1.0, 1.0))

--TODO:HUD indicator showing other users off screen (maybe on screen?)


--Table containing 7 screen positions
screenPositions = {420, 380, 340, 300, 260, 220, 180}
	
	



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

function currentPlace( following )
	if following == 0 then
		return 1
	elseif following == 1 then
		return 2
	elseif following == 2 then
		return 3
	elseif following == 3 then
		return 4
	end
end

function getPlace( playerIndex )
	local status = Engine.GetInstance().race:status(Player.players[playerIndex-1])
	return currentPlace(status.following)
end

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
	local checkpointsPerLap = Engine.GetInstance().race.course.path.Count
	local checkpoints = checkpointsPerLap * Engine.GetInstance().race.laps
	playerHUDs[playerIndex].healthBar.update(Player.players[playerIndex-1].health / Player.players[playerIndex-1].maxHealth)
	playerHUDs[playerIndex].shieldBar.update(Player.players[playerIndex-1].shield / Player.players[playerIndex-1].maxShield)
	playerHUDs[playerIndex].speedBar.update(Player.players[playerIndex-1].velocity:pos():Length() / 8.75)
	if Engine.GetInstance().secondsToStart > 0 then
		local sts = Engine.GetInstance().secondsToStart
		local int = MSMath.Truncate(sts)
		local dec = MSMath.Truncate(sts * 10) % 10
		playerHUDs[playerIndex].statusText.text = "Race starts in... " .. int .. "." .. dec
		for p = 1,4 do
			if Player.players[p-1] == nil then
					playerHUDs[playerIndex].placeIcons[p].Blink()
					playerHUDs[playerIndex].finishBars[p].visible( false )
					playerHUDs[playerIndex].finishIcons[p].Blink()
			end
		end
	else
		local status = Engine.GetInstance().race:status(Player.players[playerIndex-1])
		if status.place == 0 then
			for p = 1,4 do
				if Player.players[p-1] ~= nil then
					local pStatus = status
					if p ~= playerIndex then
						pStatus = Engine.GetInstance().race:status(Player.players[p-1]) 
					end
					local relPlace = (pStatus.checkpoint + (checkpointsPerLap * (pStatus.lap - 1))) - (status.checkpoint +  (checkpointsPerLap * (status.lap - 1)))
					if relPlace < -3 then
						relPlace = -3
					elseif relPlace > 3 then
						relPlace = 3
					end
					relPlace = relPlace + 4
					playerHUDs[playerIndex].placeIcons[p].relocateY( screenPositions[relPlace] )
					playerHUDs[playerIndex].finishBars[p].update((pStatus.checkpoint + (checkpointsPerLap * (pStatus.lap - 1))) / checkpoints)
				end
			end
			playerHUDs[playerIndex].statusText.text = "Lap: " .. status.lap .. "  CP: " .. status.checkpoint .. "  Leading: " .. status.leading .. "  Following: " .. status.following
		else
			if status.place == 1 then
				playerHUDs[playerIndex].statusText.text = "Congratulations! You won!"
			else
				playerHUDs[playerIndex].statusText.text = "Place: " .. status.place 
			end
		end
	end
end

return playerHUDs[playerHudIndex].interface