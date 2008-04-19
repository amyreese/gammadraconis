if PositionArrow == nil then
	PositionArrow = {}
	PositionArrow.positionArrowTexture = "Resources/Textures/HUD/Elements"
	function PositionArrow.new(playerIndex)
		local arrow = {}
		arrow.sprite = Sprite(GammaDraconis, Rectangle(64 * playerIndex,0,64,64))
		arrow.sprite.RelativeScale = Vector2(0.5, 0.5);
		arrow.sprite.RelativePosition = Vector2( 870, 700 )
		arrow.sprite.textureName = PositionArrow.positionArrowTexture
		function arrow.addToInterface(interface)
			interface:AddComponent(arrow.sprite)
		end
		function arrow.relocate( position )
			arrow.sprite.RelativePosition = position
		end
		function arrow.relocateY( num )
			arrow.sprite.RelativePosition = Vector2(arrow.sprite.RelativePosition.X, num)
		end
		function arrow.rotate( amt )
			arrow.sprite.RelativeRotation = amt
		end
		function arrow.nextMapPoint( mapPoint )
			arrow.sprite.RelativePosition = Vector2(mapPoint.X - arrow.sprite.RelativePosition.X, mapPoint.Y - arrow.sprite.RelativePosition.Y)
		end
		function arrow.Blink()
			arrow.sprite.Visible = false
		end
		function arrow.rescale( amt )
			arrow.sprite.RelativeScale = amt 
		end
		return arrow
	end
end
