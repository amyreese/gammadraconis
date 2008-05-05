local radius = 2500
local checkpoints = 16
local degreesBetweenCheckpoints = 360 / checkpoints

gameScene = Scene()
Engine.GetInstance().gameScene = gameScene

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
Skybox.lights[0] = Light(Vector3(-0.05,  0.1, -1), Vector3(0.9, 0.7, 0.7), Vector3(1,1,1))
Skybox.lights[1] = Light(Vector3( 0.95, -0.9,  1), Vector3(0.4, 0.4, 0.4), Vector3(0.5,0.5,0.5))

roid = Proto.getThing("Asteroid800A", Coords(2000,5,-500), Coords(1.5,-0,0,0.004,0.001,0.0004))
gameScene:track(roid, GO_TYPE.DEBRIS)

roid = Proto.getThing("Asteroid800A", Coords(3000,-5,-500), Coords(-1.5,0,0,0.003,0.004,0.0002))
gameScene:track(roid, GO_TYPE.DEBRIS)

roid = Proto.getThing("Asteroid800A", Coords(-500,5,2600), Coords(10.5,-0,0,0.004,0.001,0.0004))
gameScene:track(roid, GO_TYPE.DEBRIS)

roid = Proto.getThing("Asteroid800A", Coords(500,-5,2600), Coords(-10.5,0,0,0.003,0.004,0.0002))
gameScene:track(roid, GO_TYPE.DEBRIS)

roid = Proto.getThing("Asteroid800A", Coords(-2000,5,500), Coords(1.5,-0,0,0.004,0.001,0.0004))
gameScene:track(roid, GO_TYPE.DEBRIS)

roid = Proto.getThing("Asteroid800A", Coords(-3000,-5,500), Coords(-1.5,0,0,0.003,0.004,0.0002))
gameScene:track(roid, GO_TYPE.DEBRIS)

roid = Proto.getThing("Asteroid800B", Coords(-2800,5,500), Coords(0,0,1.5, 0.001, 0.003, 0.001))
gameScene:track(roid, GO_TYPE.DEBRIS)

roid = Proto.getThing("Asteroid800B", Coords(-2000,5,0), Coords(0,0,-1.5, 0.002, 0.001, 0.003))
gameScene:track(roid, GO_TYPE.DEBRIS)

--tunnel = Proto.getThing("AsteroidTunnel", Coords(-2000, 0, 500))
--tunnel.fakeTransparency = 0;
--gameScene:track(tunnel, GO_TYPE.GHOST)

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
		checkpointPosition = checkpointPosition + 1
		checkpoint = Checkpoint()
		checkpointscale = 0.5
		checkpoint.models:Add(FBXModel("Resources/Models/Checkpoint", "", checkpointscale))
		checkpoint.models:Add(FBXModel("Resources/Models/Checkpoint2", "", checkpointscale))
		checkpoint.size = 210 * checkpointscale
		checkpoint.position = position 
		checkpoint.racePosition = checkpointPosition
		course.path:Add( checkpoint )
		gameScene:track(checkpoint, GO_TYPE.CHECKPOINT);
	end
end

racers = Engine.GetInstance().players
for i = 0, racers.Length-1 do
	gameScene:track(racers[i], GO_TYPE.RACER)
	--racers[i].position = Coords(path[1].x - (4 + 2 * i) * racers[i].size, path[1].y, path[1].z + 2 * racers[i].size, path[1].pitch, path[1].yaw, path[1].roll)
	racers[i].position = Coords()
end

course.loop = true

race = Race(course, 3, racers)
Engine.GetInstance().race = race