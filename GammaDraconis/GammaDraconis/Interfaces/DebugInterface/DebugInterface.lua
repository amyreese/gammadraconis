debugInterface = Interface(GammaDraconis)
debugInterface.UpdateCall = 'debugInterfaceUpdate'

debugFramerateText = Text(GammaDraconis)
debugFramerateText.spriteFontName = "Resources/Fonts/Menu"
debugFramerateText.color = Color.White
debugFramerateText.text = "Test"

debugInterface:AddComponent(debugFramerateText)
debugInterface.Enabled = true

function debugInterfaceUpdate(gameTime)
	if GammaDraconis.debugFramerate then
		debugFramerateText.text = GammaDraconis.lastAverageFramerate
	else
		debugFramerateText.text = ''
	end
end

return debugInterface