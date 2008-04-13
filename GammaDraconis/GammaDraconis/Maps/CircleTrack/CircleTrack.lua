local radius = 5000
local checkpoints = 32
local degreesBetweenCheckpoints = 360 / checkpoints

gameScene = Scene()
Engine.GetInstance().gameScene = gameScene

ship = Proto.getShip("Raptor")
p = Player.cloneShip(ship, PlayerIndex.One)
p.position = Coords(radius - 4 * p.size, - 4 * p.size, -p.size, MSMath.PI, 0, 0)
gameScene:track(p, GO_TYPE.RACER)

p2 = Player.cloneShip(ship, PlayerIndex.Two)
p2.position = Coords(radius + 4 * p2.size, - 4 * p2.size, -p2.size, MSMath.PI, 0, 0)
gameScene:track(p2, GO_TYPE.RACER)

p3 = Player.cloneShip(ship, PlayerIndex.Three)
p3.position = Coords(radius - 4 * p3.size, 4 * p3.size, -p3.size, MSMath.PI, 0, 0)
gameScene:track(p3, GO_TYPE.RACER)

p4 = Player.cloneShip(ship, PlayerIndex.Four)
p4.position = Coords(radius + 4 * p4.size, 4 * p4.size, -p4.size, MSMath.PI, 0, 0)
gameScene:track(p4, GO_TYPE.RACER)

racers = Racer[4]
racers[0] = p
racers[1] = p2
racers[2] = p3
racers[3] = p4

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