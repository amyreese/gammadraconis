gameScene = Scene()
Engine.GetInstance().gameScene = gameScene

ship = Proto.getShip("Raptor")
p = Player.cloneShip(ship, PlayerIndex.One)
p.position = Coords(0.0, -5.0, 0.0, MSMath.PI, 0, 0)
gameScene:track(p, GO_TYPE.RACER)

p4 = Player.cloneShip(ship, PlayerIndex.Four)
p4.position = Coords(0.0, 5.0, 0.0, MSMath.PI, 0, 0)
gameScene:track(p4, GO_TYPE.RACER)
racers = Racer[2]
racers[0] = p
racers[1] = p4

planet = GameObject()
planet.position = Coords(-1000, 0, 50)
planet.models:Add(FBXModel("Resources/Models/Planet", "", 500))
gameScene:track(planet, GO_TYPE.SCENERY)

skybox = GameObject()
skybox.models:Add(FBXModel("Resources/Models/Skybox", "", 500*10000))
gameScene:track(skybox, GO_TYPE.SKYBOX)

course = Course()
Engine.GetInstance().course = course

roid = Proto.getThing("Asteroid800A", Coords(-1000,0,50), Coords(0,0,0,0.004,0.001,0.0004))
gameScene:track(roid, GO_TYPE.DEBRIS)

roid = Proto.getThing("Asteroid800B", Coords(-1000,0,-200), Coords(0,0,.05, 0.001, 0.003, 0.001))
gameScene:track(roid, GO_TYPE.DEBRIS)

path = {}

for i = 0, 330, 30 do
	local rad = MathHelper.ToRadians(i)
	local x = MSMath.Cos(rad)
	local y = MSMath.Sin(rad)
	table.insert( path, {x=x*1000 - 1000, y=0, z=y*1000 + 50, pitch=0, yaw=-rad, roll=0} )
end
-- TODO: Find a way to add intermediate points for AI
for i,v in ipairs( path ) do
	local position = Coords( v.x, v.y, v.z, v.pitch, v.yaw, v.roll)
	if not v.path then
		course.path:Add(position)

		checkpoint = GameObject()
		checkpoint.position = position
		checkpoint.size = 25
		checkpoint.models:Add(FBXModel("Resources/Models/Checkpoint", "", 10))
		gameScene:track(checkpoint, GO_TYPE.HUD)
	end
end

course.loop = true

race = Race(course, 1, racers)
Engine.GetInstance().race = race