local tunnelRadius = 220

gameScene = Scene()
Engine.GetInstance().gameScene = gameScene


course = Course()
Engine.GetInstance().course = course

path = {
	{x=0, y=0, z=0, yaw=0, pitch=0, roll=0 },
	{x=50, y=50, z=-400, yaw=0, pitch=0, roll=0},
	{x=150, y=-75, z=-1000, yaw=0, pitch=0, roll=0},
	{x=275, y=-300, z=-1600, yaw=MathHelper.PiOver2 / 4, pitch=0, roll=0},
	{x=150, y=-250, z=-2000, yaw=MathHelper.PiOver4, pitch=0, roll=0},
	{x=-350, y=400, z=-2500, yaw=MathHelper.PiOver2 / 8, pitch=MathHelper.PiOver2, roll=0},
	{x=-400, y=700, z=-2900, yaw=-MathHelper.PiOver4 / 4, pitch=MathHelper.PiOver2 / 8, roll=0},
	{x=-75, y=830, z=-3500, yaw=0, pitch=0, roll=0},
	}

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

--[[
	{x=130, y=-260, z=-2000, yaw=MathHelper.PiOver4, pitch=0, roll=0},
	{x=-400, y=400, z=-2500, yaw=MathHelper.PiOver4, pitch=MathHelper.PiOver2, roll=0},
	{x=-300, y=700, z=-2900, yaw=-MathHelper.PiOver4, pitch=0, roll=0},
	{x=-75, y=830, z=-3500, yaw=0, pitch=0, roll=0},
]]--

course.loop = false

tunnel = Proto.getThing("AsteroidTunnel", Coords(0, 0, -2000))
tunnel.fakeTransparency = 0;
gameScene:track(tunnel, GO_TYPE.GHOST)

--[[
room1 = Room()
room1.area = BoundingBox(Vector3(path[2].x - tunnelRadius, path[2].y - tunnelRadius, path[2].z - 3 * tunnelRadius), Vector3(path[2].x + tunnelRadius, path[2].y + tunnelRadius, path[2].z))
room1.canSeeOutside = true
gameScene.rooms:Add(room1)

room2 = Room()
room2.area = BoundingBox(Vector3(path[4].x - tunnelRadius, path[4].y - tunnelRadius, path[4].z - 3 * tunnelRadius), Vector3(path[4].x + tunnelRadius, path[4].y + tunnelRadius, path[4].z + 3 * tunnelRadius))
room2.canSeeOutside = true
room2.visibleRooms:Add(room1)
room1.visibleRooms:Add(room2)
gameScene.rooms:Add(room2)

room3 = Room()
room3.area = BoundingBox(Vector3(path[6].x - tunnelRadius, path[6].y - tunnelRadius, path[6].z - 2 * tunnelRadius), Vector3(path[6].x + tunnelRadius, path[6].y + tunnelRadius, path[6].z + 2 * tunnelRadius))
room3.canSeeOutside = false
room3.visibleRooms:Add(room2)
room2.visibleRooms:Add(room3)
gameScene.rooms:Add(room3)

room4 = Room()
room4.area = BoundingBox(Vector3(path[8].x - tunnelRadius, path[8].y - tunnelRadius, path[8].z - 2 * tunnelRadius), Vector3(path[8].x + tunnelRadius, path[8].y + tunnelRadius, path[8].z + 2 * tunnelRadius))
room4.canSeeOutside = false
room4.visibleRooms:Add(room2)
room4.visibleRooms:Add(room3)
room2.visibleRooms:Add(room4)
room3.visibleRooms:Add(room4)
gameScene.rooms:Add(room4)

room5 = Room()
room5.area = BoundingBox(Vector3(path[9].x - tunnelRadius, path[9].y - tunnelRadius, path[9].z - tunnelRadius), Vector3(path[9].x + 2 * tunnelRadius, path[9].y + tunnelRadius, path[9].z + tunnelRadius))
room5.canSeeOutside = false
room5.visibleRooms:Add(room3)
room5.visibleRooms:Add(room4)
room3.visibleRooms:Add(room5)
room4.visibleRooms:Add(room5)
gameScene.rooms:Add(room5)

room6 = Room()
room6.area = BoundingBox(Vector3(path[9].x - 6 * tunnelRadius, path[9].y - tunnelRadius, path[9].z - tunnelRadius), Vector3(path[9].x - tunnelRadius, path[9].y + tunnelRadius, path[9].z + tunnelRadius))
room6.canSeeOutside = false
room6.visibleRooms:Add(room4)
room6.visibleRooms:Add(room5)
room4.visibleRooms:Add(room6)
room5.visibleRooms:Add(room6)
gameScene.rooms:Add(room6)

room7 = Room()
room7.area = BoundingBox(Vector3(path[10].x - tunnelRadius, path[10].y - tunnelRadius, path[10].z), Vector3(path[10].x + tunnelRadius, path[10].y + tunnelRadius, path[10].z + 6 * tunnelRadius))
room7.canSeeOutside = false
room7.visibleRooms:Add(room5)
room7.visibleRooms:Add(room6)
room5.visibleRooms:Add(room7)
room6.visibleRooms:Add(room7)
gameScene.rooms:Add(room7)

room8 = Room()
room8.area = BoundingBox(Vector3(path[11].x - tunnelRadius, path[11].y - tunnelRadius, path[11].z), Vector3(path[11].x + tunnelRadius, path[11].y + tunnelRadius, path[11].z + tunnelRadius))
room8.canSeeOutside = true
room8.visibleRooms:Add(room6)
room8.visibleRooms:Add(room7)
room6.visibleRooms:Add(room8)
room7.visibleRooms:Add(room8)
gameScene.rooms:Add(room8)
]]--

racers = Engine.GetInstance().players

for i = 0, racers.Length-1 do
	gameScene:track(racers[i], GO_TYPE.RACER)
	racers[i].position = Coords(path[1].x - (4 + 2 * i) * racers[i].size, path[1].y, path[1].z + 2 * racers[i].size, path[1].pitch, path[1].yaw, path[1].roll)
end

planet = GameObject()
planet.position = Coords(0, 0, -500)
planet.size = 1000
planet.models:Add(FBXModel("Resources/Models/Planet", "", 4))
--gameScene:track(planet, GO_TYPE.SCENERY)

skybox = Skybox()
gameScene:track(skybox, GO_TYPE.SKYBOX)

roid = Proto.getThing("Asteroid800A", Coords(- 500,0,0))
roid.invincible = true
gameScene:track(roid, GO_TYPE.DEBRIS)



race = Race(course, 1, racers)
Engine.GetInstance().race = race