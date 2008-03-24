debugInterface = Interface(GammaDraconis)
debugInterface.UpdateCall = 'di.debugInterfaceUpdate'

di = {}
di.font = "Resources/Fonts/Menu"
di.color = 1
di.colors = { Color.White, Color.Red, Color.Green, Color.Blue, Color.Black }

di.debugInput = Input()
di.debugInput.inputKeys:Add("ShowDebugKeys","alt+d")
di.debugInput.inputKeys:Add("ToggleDebugFramerate","alt+f")
di.debugInput.inputKeys:Add("ToggleDebugPlayerPosition","alt+p")
di.debugInput.inputKeys:Add("ChangeDebugColor","alt+c")

di.debugTexts = {}

di.debugFramerateText = Text(GammaDraconis)
di.debugFramerateText.RelativePosition = Vector2(960, 0)
table.insert( di.debugTexts, di.debugFramerateText )

di.debugKeysText = Text(GammaDraconis)
table.insert( di.debugTexts, di.debugKeysText )

di.debugPlayerPositionInterface = Interface(GammaDraconis)
di.debugPlayerOnePosition = Text(GammaDraconis)
di.debugPlayerPositionInterface:AddComponent(di.debugPlayerOnePosition)
table.insert( di.debugTexts, di.debugPlayerOnePosition )

di.debugPlayerTwoPosition = Text(GammaDraconis)
di.debugPlayerPositionInterface:AddComponent(di.debugPlayerTwoPosition)
table.insert( di.debugTexts, di.debugPlayerTwoPosition )

di.debugPlayerThreePosition = Text(GammaDraconis)
di.debugPlayerPositionInterface:AddComponent(di.debugPlayerThreePosition)
table.insert( di.debugTexts, di.debugPlayerThreePosition )

di.debugPlayerFourPosition = Text(GammaDraconis)
di.debugPlayerPositionInterface:AddComponent(di.debugPlayerFourPosition)
table.insert( di.debugTexts, di.debugPlayerFourPosition )

di.debugPlayerPositionInterface.RelativePosition = Vector2(0, 0)

di.debugPlayerPositions = false

di.debugKeysTextValue = ""
enum = di.debugInput.inputKeys:GetEnumerator()
while enum:MoveNext() do
	di.debugKeysTextValue = di.debugKeysTextValue .. enum.Current.Key .. ": " .. enum.Current.Value .. "\n"
end

debugInterface:AddComponent(di.debugFramerateText)
debugInterface:AddComponent(di.debugKeysText)
debugInterface:AddComponent(di.debugPlayerPositionInterface)
debugInterface.Enabled = true
debugInterface.RelativePosition = Vector2(5,0)

function di.debugInterfaceUpdate(gameTime)
	Input.update()
	for i,v in ipairs(di.debugTexts) do
		v.text = ""
		v.color = di.colors[di.color]
		v.spriteFontName = di.font
	end

	if di.debugInput:inputPressed( "ToggleDebugFramerate" ) then
		GammaDraconis.debugFramerate = GammaDraconis.debugFramerate == false
	end
	if di.debugInput:inputPressed( "ToggleDebugPlayerPosition" ) then
		di.debugPlayerPositions = di.debugPlayerPositions == false
	end
	if di.debugInput:inputPressed( "ChangeDebugColor" ) then
		if di.color == table.getn(di.colors) then
			di.color = 1
		else
			di.color = di.color + 1
		end
	end
	if di.debugInput:inputDown( "ShowDebugKeys" ) then
		di.debugKeysText.text = di.debugKeysTextValue
	else
		if GammaDraconis.debugFramerate then
			di.debugFramerateText.text = GammaDraconis.lastAverageFramerate
		end
		if di.debugPlayerPositions then
			local y = 0
			if Player.players[0] ~= nil then
				di.debugPlayerOnePosition.RelativePosition = Vector2(0,y)
				di.debugPlayerOnePosition.text = "P1: ( " .. Player.players[0].position:pos().X .. ", " .. Player.players[0].position:pos().Y .. ", " .. Player.players[0].position:pos().Z .. " )"
				y = y + 32
			end
			if Player.players[1] ~= nil then
				di.debugPlayerTwoPosition.RelativePosition = Vector2(0,y)
				di.debugPlayerTwoPosition.text = "P2: ( " .. Player.players[1].position:pos().X .. ", " .. Player.players[1].position:pos().Y .. ", " .. Player.players[1].position:pos().Z .. " )"
				y = y + 32
			end
			if Player.players[2] ~= nil then
				di.debugPlayerThreePosition.RelativePosition = Vector2(0,y)
				di.debugPlayerThreePosition.text = "P3: ( " .. Player.players[2].position:pos().X .. ", " .. Player.players[2].position:pos().Y .. ", " .. Player.players[2].position:pos().Z .. " )"
				y = y + 32
			end
			if Player.players[3] ~= nil then
				di.debugPlayerFourPosition.RelativePosition = Vector2(0,y)
				di.debugPlayerFourPosition.text = "P4: ( " .. Player.players[3].position:pos().X .. ", " .. Player.players[3].position:pos().Y .. ", " .. Player.players[3].position:pos().Z .. " )"
				y = y + 32
			end
		end
	end
end

return debugInterface