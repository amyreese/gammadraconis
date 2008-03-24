debugInterface = Interface(GammaDraconis)
debugInterface.UpdateCall = 'di.debugInterfaceUpdate'

di = {}
di.debugInput = Input()
di.debugInput.inputKeys:Add("ShowDebugKeys","alt+d")
di.debugInput.inputKeys:Add("ToggleDebugFramerate","alt+f")

di.debugFramerateText = Text(GammaDraconis)
di.debugFramerateText.spriteFontName = "Resources/Fonts/Menu"
di.debugFramerateText.color = Color.White
di.debugFramerateText.text = "Test"

di.debugKeysText = Text(GammaDraconis)
di.debugKeysText.spriteFontName = "Resources/Fonts/Menu"
di.debugKeysText.color = Color.White
di.debugKeysText.text = ""

di.debugKeysTextValue = ""
enum = di.debugInput.inputKeys:GetEnumerator()
while enum:MoveNext() do
	di.debugKeysTextValue = di.debugKeysTextValue .. enum.Current.Key .. ": " .. enum.Current.Value .. "\n"
end

debugInterface:AddComponent(di.debugFramerateText)
debugInterface:AddComponent(di.debugKeysText)
debugInterface.Enabled = true
debugInterface.RelativePosition = Vector2(5,0)

function di.debugInterfaceUpdate(gameTime)
	Input.update()
	di.debugKeysText.text = ""
	di.debugFramerateText.text = ""

	if di.debugInput:inputPressed( "ToggleDebugFramerate" ) then
		GammaDraconis.debugFramerate = GammaDraconis.debugFramerate == false
	end
	if di.debugInput:inputDown( "ShowDebugKeys" ) then
		di.debugKeysText.text = di.debugKeysTextValue
	else
		if GammaDraconis.debugFramerate then
			di.debugFramerateText.text = GammaDraconis.lastAverageFramerate
		end
	end
end

return debugInterface