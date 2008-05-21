library( "MapBuilders/Turns" )
library( "MapBuilders/AsteroidFields" )



gameScene = Scene()
Engine.GetInstance().gameScene = gameScene

planet = Proto.getThing("Mars")
planet.position = Coords(0, -4000, 0)
gameScene:track(planet, GO_TYPE.SCENERY)

--tunnel = GameObject()
--tunnel.position = Coords(1100,5,5)
--tunnel.size = 100
--tunnel.models:Add(FBXModel("Resources/Models/Tunnel", "", 100))
--gameScene:track(tunnel, GO_TYPE.COLLIDABLE)

skybox = Skybox()
gameScene:track(skybox, GO_TYPE.SKYBOX)
Skybox.lights[0] = Light(Vector3(-0.05,  0.1, -1), Vector3(0.9, 0.7, 0.7), Vector3(1,1,1))
Skybox.lights[1] = Light(Vector3( 0.95, -0.9,  1), Vector3(0.4, 0.4, 0.4), Vector3(0.5,0.5,0.5))

makeRoidRow( -1000, 7250, 6500, gameScene )
--makeRoidPlane( -1000, 7500, 6500, gameScene, 2 )
--makeRoidCube( -1000, 7000, 6500, gameScene, 2 )



course = Course()
Engine.GetInstance().course = course

--TODO: Build tools that create left turns, right turns, up turns and down turns, and perhaps some mixes?  Spirals?
path = {}
xoffset = -1500
yoffset = 0
zoffset = 500
local trackAttributes = {radius=2500, checkpoints = 16, degreesBetweenCheckpoints = 360 / 16}

--All Zero Checkpoint for proper axis relation
--table.insert( path, {x=0, y=0, z=0, pitch=0, yaw=0, roll=0} )

--path = PosYToPosZ( path, xoffset, yoffset, zoffset, trackAttributes )
--path = PosZToPosY(path, xoffset, 5000, 500, trackAttributes)
path = PosYToPosZ( path, xoffset, 5500, 5500, trackAttributes )
--path = NegYtoPosZ( path, xoffset, yoffset, 500, trackAttributes )
--path = NegYToNegZ(path, xoffset, -500, 2500, trackAttributes)
--path = NegYToPosZ( path, xoffset, 1500, 3500, trackAttributes )
--path = PosZToNegX(path, xoffset, yoffset, zoffset, trackAttributes)
--path = PosZToPosX(path, 2500, yoffset, 2000, trackAttributes)

table.insert( path, {x=-1500, y=8500, z=12000, pitch=0, yaw=0, roll=0} )

-- TODO: Find a way to add intermediate points for AI
for i,v in ipairs( path ) do
	local position = Coords( v.x, v.y, v.z, v.pitch, v.yaw, v.roll)
	if not v.path then
		course.path:Add( position )
		course.types:Add( "Checkpoint" )
	end
end


course.loop = true
course.laps = 3