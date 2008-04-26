local tunnelRadius = 220

gameScene = Scene()
Engine.GetInstance().gameScene = gameScene


course = Course()
Engine.GetInstance().course = course

path = {
	{x=0, y=0, z=0, yaw=0, pitch=0, roll=0 },
	{x=0, y=0, z=-400, yaw=0, pitch=0, roll=0},
	{x=-200, y=0, z=-800, yaw=MathHelper.PiOver2, pitch=0, roll=0},
	{x=-400, y=0, z=-1200, yaw=0, pitch=0, roll=0},
	{x=-200, y=0, z=-1600, yaw=-MathHelper.PiOver2, pitch=0, roll=0},
	{x=0, y=0, z=-1400, yaw=MathHelper.Pi, pitch=0, roll=0},
	{x=200, y=0, z=-1200, yaw=-MathHelper.PiOver2, pitch=0, roll=0},
	{x=400, y=0, z=-1400, yaw=0, pitch=0, roll=0},
	{x=200, y=0, z=-2000, yaw=MathHelper.PiOver2, pitch=0, roll=0},
	{x=-800, y=0, z=-1800, yaw=MathHelper.Pi, pitch=0, roll=0},
	{x=-1000, y=0, z=-400, yaw=MathHelper.PiOver2, pitch=0, roll=0},
	}

-- TODO: Find a way to add intermediate points for AI
checkpointPosition = 0;
for i,v in ipairs( path ) do
	local position = Coords( v.x, v.y, v.z, v.pitch, v.yaw, v.roll)
	if not v.path then
		course.path:Add(position)
		checkpointPosition = checkpointPosition + 1;
		checkpoint = Checkpoint()
		checkpoint.position = position
		checkpoint.models:Add(FBXModel("Resources/Models/Checkpoint", "", 0.195))
		gameScene:track(checkpoint, GO_TYPE.HUD)
	end
end

local spinMod = 0
function buildAsteroidTunnel( x, y, z, radius, rotation )
	local rotMod = MathHelper.PiOver2 * spinMod
	spinMod = 1 - spinMod
	for i=0,10 do
		local rad = MathHelper.Pi * (i / 5) + rotMod
		local pos = Vector3( MSMath.Cos(rad) * radius, MSMath.Sin(rad) * radius, 0 )
		pos = Matrix.Multiply(Matrix.CreateTranslation(pos), Matrix.CreateRotationY(rotation)).Translation
		local roid = Proto.getThing("Asteroid800A", Coords(pos.X + x, pos.Y + y, pos.Z + z))
		roid.immobile = true
		roid:scaleModels(0.6)
		roid.size = roid.size * 0.6
		roid.invincible = true
		gameScene:track(roid, GO_TYPE.DEBRIS)
	end
end

local radius = 210
buildAsteroidTunnel( 0, 0, -400, radius, 0 );
buildAsteroidTunnel( 0, 0, -500, radius, 0 );
buildAsteroidTunnel( 0, 0, -600, radius, 0 );
buildAsteroidTunnel( -40, 0, -640, radius, MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( -80, 0, -680, radius, 2 * MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( -120, 0, -720, radius, 3 * MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( -160, 0, -760, radius, 4 * MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( -200, 0, -800, radius, MathHelper.PiOver2 );
buildAsteroidTunnel( -240, 0, -840, radius, 4 * MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( -280, 0, -880, radius, 3 * MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( -320, 0, -920, radius, 2 * MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( -360, 0, -960, radius, MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( -400, 0, -1000, radius, 0 );
buildAsteroidTunnel( -400, 0, -1100, radius, 0 );
buildAsteroidTunnel( -400, 0, -1200, radius, 0 );
buildAsteroidTunnel( -400, 0, -1300, radius, 0 );
buildAsteroidTunnel( -400, 0, -1400, radius, 0 );
buildAsteroidTunnel( -360, 0, -1440, radius, -MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( -320, 0, -1480, radius, -2 * MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( -280, 0, -1520, radius, -3 * MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( -240, 0, -1560, radius, -4 * MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( -200, 0, -1600, radius, -MathHelper.PiOver2 );
buildAsteroidTunnel( -160, 0, -1560, radius, -MathHelper.PiOver2 / 5 - MathHelper.PiOver2 );
buildAsteroidTunnel( -120, 0, -1520, radius, -2 * MathHelper.PiOver2 / 5 - MathHelper.PiOver2 );
buildAsteroidTunnel( -80, 0, -1480, radius, -3 * MathHelper.PiOver2 / 5 - MathHelper.PiOver2 );
buildAsteroidTunnel( -40, 0, -1440, radius, -4 * MathHelper.PiOver2 / 5 - MathHelper.PiOver2 );
buildAsteroidTunnel( 0, 0, -1400, radius, -MathHelper.Pi );
buildAsteroidTunnel( 40, 0, -1360, radius, -4 * MathHelper.PiOver2 / 5 - MathHelper.PiOver2 );
buildAsteroidTunnel( 80, 0, -1320, radius, -3 * MathHelper.PiOver2 / 5 - MathHelper.PiOver2 );
buildAsteroidTunnel( 120, 0, -1280, radius, -2 * MathHelper.PiOver2 / 5 - MathHelper.PiOver2 );
buildAsteroidTunnel( 160, 0, -1240, radius, -MathHelper.PiOver2 / 5 - MathHelper.PiOver2 );
buildAsteroidTunnel( 200, 0, -1200, radius, -MathHelper.PiOver2 );
buildAsteroidTunnel( 240, 0, -1240, radius, -4 * MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( 280, 0, -1280, radius, -3 * MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( 320, 0, -1320, radius, -2 * MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( 360, 0, -1360, radius, -MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( 400, 0, -1400, radius, 0 );
buildAsteroidTunnel( 400, 0, -1500, radius, 0 );
buildAsteroidTunnel( 400, 0, -1600, radius, 0 );
buildAsteroidTunnel( 400, 0, -1700, radius, 0 );
buildAsteroidTunnel( 400, 0, -1800, radius, 0 );
buildAsteroidTunnel( 360, 0, -1840, radius, MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( 320, 0, -1880, radius, 2 * MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( 280, 0, -1920, radius, 3 * MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( 240, 0, -1960, radius, 4 * MathHelper.PiOver2 / 5 );
buildAsteroidTunnel( 200, 0, -2000, radius, MathHelper.PiOver2 );

--[[
	{x=200, y=0, z=-2000, yaw=MathHelper.PiOver2, pitch=0, roll=0},
	{x=-800, y=0, z=-1800, yaw=MathHelper.Pi, pitch=0, roll=0},
	{x=-1000, y=0, z=-400, yaw=MathHelper.PiOver2, pitch=0, roll=0},
]]--

course.loop = false

--tunnel = GameObject()
--tunnel.position = Coords(500,0,-500)
--tunnel.size = 100
--tunnel.models:Add(FBXModel("Resources/Models/Tunnel", "", 50))
--gameScene:track(tunnel, GO_TYPE.THINKABLE)

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

skybox = GameObject()
skybox.models:Add(FBXModel("Resources/Models/Skybox", "", 0.5))
gameScene:track(skybox, GO_TYPE.SKYBOX)

roid = Proto.getThing("Asteroid800A", Coords(- 500,0,0))
roid.invincible = true
gameScene:track(roid, GO_TYPE.DEBRIS)



race = Race(course, 1, racers)
Engine.GetInstance().race = race