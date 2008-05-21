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
		bar.icon = Sprite(GammaDraconis)
		bar.status.textureName = StatusBar.statusTexture
		bar.overlay.textureName = StatusBar.overlayTexture
		bar.background.textureName = StatusBar.backgroundTexture 
		function bar.addToInterface( interface )
			interface:AddComponent(bar.background)
			interface:AddComponent(bar.status)
			interface:AddComponent(bar.overlay)
			interface:AddComponent(bar.icon)
		end
		function bar.setIcon( icon )
			bar.icon.textureName = icon;
			bar.icon.RelativeScale = Vector2(0.25, 0.25)
		end
		function bar.relocate( position )
			bar.background.RelativePosition = position
			bar.status.RelativePosition = position
			bar.overlay.RelativePosition = position
			bar.icon.RelativePosition = Vector2.Subtract(position, Vector2(40, 8) )
		end
		function bar.update( percentage )
			bar.status.RelativeScale = Vector2( percentage, 1 )
		end
		function bar.color( color )
			bar.background.color = color
			bar.status.color = color
		end
		function bar.visible( vis )
			bar.background.Visible = vis
			bar.status.Visible = vis
			bar.overlay.Visible = vis
			bar.icon.Visible = vis
		end
		return bar
	end
end