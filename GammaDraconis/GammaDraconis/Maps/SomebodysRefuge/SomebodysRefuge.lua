local tunnelRadius = 220

gameScene = Scene()
Engine.GetInstance().gameScene = gameScene


course = Course()
Engine.GetInstance().course = course

path = {
	{x=0, y=50, z=9000, yaw=0, pitch=0, roll=0 },
	{x=0, y=50, z=7000, yaw=0, pitch=0, roll=0 },
	{x=0, y=50, z=5000, yaw=0, pitch=0, roll=0 },
	{x=0, y=50, z=3000, yaw=0, pitch=0, roll=0 },
	{x=0, y=50, z=1000, yaw=0, pitch=0, roll=0 },
	{x=50, y=50, z=-400, yaw=0, pitch=0, roll=0},
	{x=150, y=-75, z=-1000, yaw=0, pitch=0, roll=0},
	{x=275, y=-300, z=-1600, yaw=MathHelper.PiOver2 / 4, pitch=0, roll=0},
	{x=150, y=-250, z=-2000, yaw=MathHelper.PiOver4, pitch=0, roll=0},
	{x=-350, y=400, z=-2500, yaw=MathHelper.PiOver2 / 8, pitch=MathHelper.PiOver2, roll=0},
	{x=-400, y=700, z=-2900, yaw=-MathHelper.PiOver4 / 4, pitch=MathHelper.PiOver2 / 8, roll=0},
	{x=-75, y=830, z=-3500, yaw=0, pitch=0, roll=0},
	}
	
local radius = 4000
local checkpoints = 8
local degreesBetweenCheckpoints = 90 / checkpoints
for i = 0, 90 - degreesBetweenCheckpoints, degreesBetweenCheckpoints do
	local rad = MathHelper.ToRadians(i)
	local x = MSMath.Cos(rad)
	local z = MSMath.Sin(rad)
	table.insert( path, {x=-75, y=x*radius + 830-radius, z=-z*radius - 4000, pitch=-rad, yaw=0, roll=0} )
end
local radius2 = 1000
checkpoints = 4
degreesBetweenCheckpoints = 145 / checkpoints
for i = 90, 235 - degreesBetweenCheckpoints, degreesBetweenCheckpoints do
	local rad = MathHelper.ToRadians(i)
	local x = MSMath.Cos(rad)
	local z = MSMath.Sin(rad)
	table.insert( path, {x=-75, y=x*radius2 + 830-radius, z=-z*radius2 - 3000 - radius, pitch=-rad, yaw=0, roll=0} )
end

-- TODO: Find a way to add intermediate points for AI
for i,v in ipairs( path ) do
	local position = Coords( v.x, v.y, v.z, v.pitch, v.yaw, v.roll)
	if not v.path then
		course.path:Add( position )
		course.types:Add( "Checkpoint" )
	end
end

local spinMod = 0
local numAsteroids = 14
function buildAsteroidTunnel( x, y, z, radius, rotationY, rotationX )
    rotationX = rotationX or 0
	local rotMod = MathHelper.PiOver2 * spinMod
	spinMod = 1 - spinMod
	for i=0,numAsteroids do
		local rad = MathHelper.Pi * (i / (numAsteroids / 2)) + rotMod
		local pos = Vector3( MSMath.Cos(rad) * radius, MSMath.Sin(rad) * radius, 0 )
		pos = Matrix.Multiply(Matrix.CreateTranslation(pos), Matrix.Multiply(Matrix.CreateRotationY(rotationY), Matrix.CreateRotationX(rotationX))).Translation
		local roid = Proto.getThing("Asteroid800A", Coords(pos.X + x, pos.Y + y, pos.Z + z))
		roid.immobile = true
		roid:scaleModels(0.6)
		roid.size = roid.size * 0.6
		roid.invincible = true
		roid.models:Clear()
		gameScene:track(roid, GO_TYPE.DEBRIS)
	end
end

local radius = 250

buildAsteroidTunnel( 50, 50, -400, radius, MathHelper.PiOver2 / 3 );
buildAsteroidTunnel( 50, 50, -450, radius, 0 );
buildAsteroidTunnel( 50, 50, -550, radius, 0 );
buildAsteroidTunnel( 50, 50, -700, radius, 0 );
buildAsteroidTunnel( 100, 0, -800, radius, 0 );
buildAsteroidTunnel( 100, -25, -900, radius, 0 );
buildAsteroidTunnel( 150, -75, -1000, radius, 0 );
buildAsteroidTunnel( 175, -100, -1100, radius, 0 );
buildAsteroidTunnel( 200, -125, -1200, radius, 0 );
buildAsteroidTunnel( 225, -200, -1300, radius, 0 );
buildAsteroidTunnel( 275, -250, -1400, radius, 0 );
buildAsteroidTunnel( 300, -250, -1500, radius, 0 );
buildAsteroidTunnel( 275, -300, -1600, radius, MathHelper.PiOver2 / 4 );
buildAsteroidTunnel( 250, -300, -1700, radius, MathHelper.PiOver2 / 4 );
buildAsteroidTunnel( 225, -300, -1800, radius, MathHelper.PiOver2 / 4 );
buildAsteroidTunnel( 200, -275, -1900, radius, MathHelper.PiOver2 / 4 );
buildAsteroidTunnel( 150, -250, -2000, radius, MathHelper.PiOver2 / 4 );

radius = 300

buildAsteroidTunnel(   100, -225, -2025, radius, MathHelper.PiOver2 / 4, MathHelper.PiOver2 / 10 );
buildAsteroidTunnel(   75, -200, -2050, radius, MathHelper.PiOver2 / 4, MathHelper.PiOver2 / 10 );
buildAsteroidTunnel(    0, -150, -2100, radius, MathHelper.PiOver2 / 4, 2 * MathHelper.PiOver2 / 10 );
buildAsteroidTunnel(  -50, -100, -2150, radius, MathHelper.PiOver2 / 4, 3 * MathHelper.PiOver2 / 10 );
buildAsteroidTunnel( -100,  -50, -2200, radius, MathHelper.PiOver2 / 4, 4 * MathHelper.PiOver2 / 10 );
buildAsteroidTunnel( -150,   -0, -2250, radius, MathHelper.PiOver2 / 4, 5 * MathHelper.PiOver2 / 10 );
buildAsteroidTunnel( -200,  100, -2300, radius, MathHelper.PiOver2 / 5, 6 * MathHelper.PiOver2 / 10 );
buildAsteroidTunnel( -250,  150, -2350, radius, MathHelper.PiOver2 / 6, 7 * MathHelper.PiOver2 / 10 );
buildAsteroidTunnel( -300,  225, -2400, radius, MathHelper.PiOver2 / 7, 8 * MathHelper.PiOver2 / 10 );
buildAsteroidTunnel( -325,  300, -2450, radius, MathHelper.PiOver2 / 8, 9 * MathHelper.PiOver2 / 10 );
buildAsteroidTunnel( -350,  400, -2500, radius, MathHelper.PiOver2 / 8, MathHelper.PiOver2 );

radius = 325

buildAsteroidTunnel( -350,  400, -2550, radius, MathHelper.PiOver2 / 8, 7 * MathHelper.PiOver2 / 8 );
buildAsteroidTunnel( -350,  450, -2600, radius, 0, 6 * MathHelper.PiOver2 / 8 );
buildAsteroidTunnel( -325,  500, -2650, radius, - MathHelper.PiOver2 / 4 / 6, 5 * MathHelper.PiOver2 / 8 );
buildAsteroidTunnel( -300,  525, -2700, radius, - 2 * MathHelper.PiOver2 / 4 / 6, 4 * MathHelper.PiOver2 / 8 );
buildAsteroidTunnel( -300,  550, -2750, radius, - 3 * MathHelper.PiOver2 / 4 / 6, 3 * MathHelper.PiOver2 / 8 );

radius = 275
buildAsteroidTunnel( -350,  600, -2800, radius, - 4 * MathHelper.PiOver2 / 4 / 6, 2 * MathHelper.PiOver2 / 8 );
buildAsteroidTunnel( -375,  650, -2850, radius, - 5 * MathHelper.PiOver2 / 4 / 6, MathHelper.PiOver2 / 8 );


radius = 240
buildAsteroidTunnel( -400,  700, -2900, radius, -MathHelper.PiOver4 / 4, MathHelper.PiOver2 / 8 );
buildAsteroidTunnel( -350,  720, -2950, radius, -MathHelper.PiOver4 / 4, MathHelper.PiOver2 / 8 );
radius = 250
buildAsteroidTunnel( -300,  740, -3000, radius, -MathHelper.PiOver4 / 4, MathHelper.PiOver2 / 8 );
buildAsteroidTunnel( -250,  760, -3050, radius, -MathHelper.PiOver4 / 4, MathHelper.PiOver2 / 8 );
buildAsteroidTunnel( -200,  780, -3100, radius, -MathHelper.PiOver4 / 4, MathHelper.PiOver2 / 8 );
buildAsteroidTunnel( -150,  810, -3150, radius, -MathHelper.PiOver4 / 4, MathHelper.PiOver2 / 8 );
buildAsteroidTunnel( -100,  840, -3200, radius, -MathHelper.PiOver4 / 5, MathHelper.PiOver2 / 7 );
buildAsteroidTunnel( -115,  860, -3250, radius, -MathHelper.PiOver4 / 5, MathHelper.PiOver2 / 7 );
buildAsteroidTunnel( -130,  880, -3300, radius, -MathHelper.PiOver4 / 6, MathHelper.PiOver2 / 6 );
buildAsteroidTunnel( -115,  860, -3350, radius, -MathHelper.PiOver4 / 6, MathHelper.PiOver2 / 6 );
radius = 200
buildAsteroidTunnel( -100,  840, -3400, radius, -MathHelper.PiOver4 / 7, MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( -80,  835, -3450, radius, -MathHelper.PiOver4 / 7, MathHelper.PiOver2 / 5 );

buildAsteroidTunnel( -75,  830, -3500, radius, MathHelper.PiOver4 / 8, MathHelper.PiOver2 / 4 );

course.loop = false

tunnel = Proto.getThing("AsteroidTunnel", Coords(0, 0, -2000))
gameScene:track(tunnel, GO_TYPE.GHOST)

room1 = Room()
room1.area = BoundingBox(Vector3(-400, -315, -2400), Vector3(400, 350, -2100))
room1.canSeeOutside = false
gameScene.rooms:Add(room1)

room2 = Room()
room2.area = BoundingBox(Vector3(-700, -600, -3500), Vector3(600, 1100, 0))
room2.canSeeOutside = true
room1.visibleRooms:Add(room2)
gameScene.rooms:Add(room2)

planet = GameObject()
planet.position = Coords(-3000, -3000, 3000)
planet.size = 1000
planet.models:Add(FBXModel("Resources/Models/Planet", "", 4))
gameScene:track(planet, GO_TYPE.SCENERY)

skybox = Skybox()
gameScene:track(skybox, GO_TYPE.SKYBOX)
Skybox.lights[0] = Light(Vector3(-0.05,  0.1, -1), Vector3(0.9, 0.7, 0.7), Vector3(1,1,1))
Skybox.lights[1] = Light(Vector3( 0.95, -0.9,  1), Vector3(0.4, 0.4, 0.4), Vector3(0.5,0.5,0.5))

roid = Proto.getThing("Asteroid800A", Coords(250, 350, 10000), Coords(-0.5, -0.5, -3))
gameScene:track(roid, GO_TYPE.DEBRIS)

roid = Proto.getThing("Asteroid800B", Coords(500, 350, 1000), Coords(-0.5, -1.5, -1, 0.0025, 0.0015, 0.005))
gameScene:track(roid, GO_TYPE.DEBRIS)

roid = Proto.getThing("Asteroid800B", Coords(1000, 0, 0), Coords(0, 0, 0, 0.0025, 0.0015, 0.005))
gameScene:track(roid, GO_TYPE.DEBRIS)
