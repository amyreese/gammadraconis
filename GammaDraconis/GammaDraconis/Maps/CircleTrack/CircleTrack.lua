local radius = 2500
local checkpoints = 16
local degreesBetweenCheckpoints = 360 / checkpoints

gameScene = Scene()
Engine.GetInstance().gameScene = gameScene

racers = Engine.GetInstance().players

for i = 0, racers.Length-1 do
	gameScene:track(racers[i], GO_TYPE.RACER)
	racers[i].position = Coords(radius - (-4 + 2.1 * i) * racers[i].size, 0, - 2 * racers[i].size, MSMath.PI, 0, 0)
end

planet = GameObject()
planet.position = Coords(0, -4000, 0)
planet.size = 1000
planet.models:Add(FBXModel("Resources/Models/Planet", "", 4))
gameScene:track(planet, GO_TYPE.SCENERY)

--tunnel = GameObject()
--tunnel.position = Coords(1100,5,5)
--tunnel.size = 100
--tunnel.models:Add(FBXModel("Resources/Models/Tunnel", "", 100))
--gameScene:track(tunnel, GO_TYPE.COLLIDABLE)

skybox = Skybox()
gameScene:track(skybox, GO_TYPE.SKYBOX)

roid = Proto.getThing("Asteroid800A", Coords(- radius / 2,5,5), Coords(1.5,-0,0,0.004,0.001,0.0004))
gameScene:track(roid, GO_TYPE.DEBRIS)

roid = Proto.getThing("Asteroid800A", Coords(radius / 2,-5,-5), Coords(-1.5,0,0,0.003,0.004,0.0002))
gameScene:track(roid, GO_TYPE.DEBRIS)

roid = Proto.getThing("Asteroid800B", Coords(5,5,- 2 * radius / 3), Coords(0,0,1.5, 0.001, 0.003, 0.001))
gameScene:track(roid, GO_TYPE.DEBRIS)

roid = Proto.getThing("Asteroid800B", Coords(-5,-5, 2 * radius / 3), Coords(0,0,-1.5, 0.002, 0.001, 0.003))
gameScene:track(roid, GO_TYPE.DEBRIS)

course = Course()
Engine.GetInstance().course = course

path = {}
for i = 0, 360 - degreesBetweenCheckpoints, degreesBetweenCheckpoints do
	local rad = MathHelper.ToRadians(i)
	local x = MSMath.Cos(rad)
	local z = MSMath.Sin(rad)
	table.insert( path, {x=x*radius, y=0, z=z*radius, pitch=0, yaw=MSMath.PI-rad, roll=0} )
end
-- TODO: Find a way to add intermediate points for AI
checkpointPosition = 0;
for i,v in ipairs( path ) do
	local position = Coords( v.x, v.y, v.z, v.pitch, v.yaw, v.roll)
	if not v.path then
		course.path:Add(position)
	end
end

course.loop = true

race = Race(course, 3, racers)
Engine.GetInstance().race = race