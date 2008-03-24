if StatusBar == nil then
	StatusBar = {}
	StatusBar.statusTexture = "Resources/Textures/HUD/StatusBarForeground"
	StatusBar.overlayTexture = "Resources/Textures/HUD/StatusBarOverlay"
	StatusBar.backgroundTexture = "Resources/Textures/HUD/StatusBarBackground"
	function StatusBar.new()
		local bar = {}
		bar.overlay = Sprite(GammaDraconis)
		bar.status = Sprite(GammaDraconis)
		bar.background = Sprite(GammaDraconis)
		bar.status.textureName = StatusBar.statusTexture
		bar.overlay.textureName = StatusBar.overlayTexture
		bar.background.textureName = StatusBar.backgroundTexture 
		function bar.addToInterface(interface)
			interface:AddComponent(bar.background)
			interface:AddComponent(bar.status)
			interface:AddComponent(bar.overlay)
		end
		function bar.relocate( position )
			bar.background.RelativePosition = position
			bar.status.RelativePosition = position
			bar.overlay.RelativePosition = position
		end
		function bar.update( percentage )
			bar.status.RelativeScale = Vector2( percentage, 1 )
		end
		return bar
	end
end