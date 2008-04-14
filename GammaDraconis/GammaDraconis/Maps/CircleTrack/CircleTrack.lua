local radius = 5000
local checkpoints = 32
local degreesBetweenCheckpoints = 360 / checkpoints

gameScene = Scene()
Engine.GetInstance().gameScene = gameScene

racers = Engine.GetInstance().players

for i = 0, racers.Length-1 do
	gameScene:track(racers[i], GO_TYPE.RACER)
	racers[i].position = Coords(radius - (4 + 2 * i) * racers[i].size, 0, - 2 * racers[i].size, MSMath.PI, 0, 0)
end

planet = GameObject()
planet.position = Coords(0, -4000, 0)
planet.size = 1000
planet.models:Add(FBXModel("Resources/Models/Planet", "", 4))
gameScene:track(planet, GO_TYPE.SCENERY)

skybox = GameObject()
skybox.models:Add(FBXModel("Resources/Models/Skybox", "", 0.5))
gameScene:track(skybox, GO_TYPE.SKYBOX)

roid = Proto.getThing("Asteroid800A", Coords(- radius / 2,0,0), Coords(0.8,-0,0,0.004,0.001,0.0004))
gameScene:track(roid, GO_TYPE.DEBRIS)

roid = Proto.getThing("Asteroid800A", Coords(radius / 2,0,0), Coords(-0.8,0,0,0.004,0.001,0.0004))
gameScene:track(roid, GO_TYPE.DEBRIS)

roid = Proto.getThing("Asteroid800B", Coords(0,0,-radius / 3), Coords(0,0,0.8, 0.001, 0.003, 0.001))
gameScene:track(roid, GO_TYPE.DEBRIS)

roid = Proto.getThing("Asteroid800B", Coords(0,0,radius / 3), Coords(0,0,-0.8, 0.001, 0.003, 0.001))
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
for i,v in ipairs( path ) do
	local position = Coords( v.x, v.y, v.z, v.pitch, v.yaw, v.roll)
	if not v.path then
		course.path:Add(position)

		checkpoint = GameObject()
		checkpoint.position = position
		checkpoint.size = 50
		checkpoint.models:Add(FBXModel("Resources/Models/Checkpoint", "", 0.5))
		gameScene:track(checkpoint, GO_TYPE.HUD)
	end
end

course.loop = true

race = Race(course, 1, racers)
Engine.GetInstance().race = race